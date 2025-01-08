using LethalNetworkAPI;

namespace DobieWan;

using GameNetcodeStuff;
using UnityEngine;

internal class GhostManager : MonoBehaviour
{
	private LethalClientMessage<HauntVictimEventData> m_hauntVictimEvent = null;
	private LethalClientMessage<VictimHauntedEventData> m_victimHauntedEvent = null;

	private GhostEventDetector m_ghostEventDetector = null;
	private GhostSfxPlayer m_ghostSfxPlayer = null; // Ideally ghost sfx player subs to this class than the inverse but who cares

	private void Start()
	{
		m_ghostEventDetector = new GhostEventDetector(TriggerHauntVictimEvent);
		m_ghostSfxPlayer = new GhostSfxPlayer();

		m_hauntVictimEvent = new LethalClientMessage<HauntVictimEventData>("HauntVictim", onReceivedFromClient: ReceiveHauntVictimEvent);
		m_victimHauntedEvent = new LethalClientMessage<VictimHauntedEventData>("VictimHaunted", onReceivedFromClient: ReceiveVictimHauntedEvent);
		
		Plugin.Log.LogInfo("GhostManager initialized!");
	}

	private void LateUpdate()
	{
		m_ghostEventDetector.Simulate();
	}

	private void TriggerHauntVictimEvent(PlayerControllerB forPlayer)
	{
		ulong forPlayerId = forPlayer.actualClientId;
		Plugin.Log.LogInfo($"Triggered haunt victim event for {forPlayerId}");

		HauntVictimEventData data = new HauntVictimEventData(forPlayerId);
		m_hauntVictimEvent.SendAllClients(data);
	}

	private void ReceiveHauntVictimEvent(HauntVictimEventData data, ulong uselessParam)
	{
		PlayerControllerB localPlayerController = StartOfRound.Instance.localPlayerController;
		Plugin.Log.LogInfo($"Haunt victim received for user {data.SpectatedUserId}. This user is {localPlayerController.actualClientId}");

		if (localPlayerController.actualClientId == data.SpectatedUserId)
		{
			string clipName = m_ghostSfxPlayer.PlayHauntedSfx();
			Plugin.Log.LogInfo("Haunt victim: Playing clip " + clipName);
			TriggerVictimHauntedEvent(clipName);
		}
	}

	private void TriggerVictimHauntedEvent(string audioClipName)
	{
		Plugin.Log.LogInfo($"Triggered victim haunted event with clip {audioClipName}");

		VictimHauntedEventData data = new VictimHauntedEventData(audioClipName, StartOfRound.Instance.localPlayerController.actualClientId);
		m_victimHauntedEvent.SendAllClients(data);
	}

	private void ReceiveVictimHauntedEvent(VictimHauntedEventData data, ulong uselessParam)
	{
		PlayerControllerB localPlayerController = StartOfRound.Instance.localPlayerController;
		if (localPlayerController.spectatedPlayerScript == null)
			return;
		
		Plugin.Log.LogInfo($"Victim haunted received from user {data.FromUserId}. The spectated user is {localPlayerController.spectatedPlayerScript.actualClientId}");
		
		ulong specatedUserId = localPlayerController.spectatedPlayerScript.actualClientId;
		if (specatedUserId == data.FromUserId)
		{
			Plugin.Log.LogInfo("Victim haunted: Playing clip: " + data.ClipName);
			m_ghostSfxPlayer.PlaySpectatorSfx(data.ClipName);
		}
	}
}