using LethalNetworkAPI;

namespace Dobes
{
	using GameNetcodeStuff;
    using UnityEngine;

	internal class GhostManager : MonoBehaviour
	{
		private LethalClientMessage<HauntVictimEventData> m_hauntVictimEvent = null;
		private LethalClientMessage<VictimHauntedEventData> m_victimHauntedEvent = null;
		
		private PlayerGhostEventDetector m_ghostEventDetector = null;
		private PlayerGhostSfxPlayer m_ghostSfxPlayer = null; // Ideally ghost sfx player subs to this class than the inverse but who cares

		private void Start()
		{
			m_ghostEventDetector = new PlayerGhostEventDetector(TriggerHauntVictimEvent);
			m_ghostSfxPlayer = new PlayerGhostSfxPlayer();
			
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

		private void ReceiveHauntVictimEvent(HauntVictimEventData data, ulong fromUser)
		{
			PlayerControllerB localPlayerController = StartOfRound.Instance.localPlayerController;
			Plugin.Log.LogInfo($"Haunt victim received for user {data.SpectatedUserId}. This user is {localPlayerController.actualClientId}");

			if (localPlayerController.actualClientId == data.SpectatedUserId)
			{
				string audioClip = m_ghostSfxPlayer.PlaySfx();
				TriggerVictimHauntedEvent(audioClip);
			}
		}

		private void TriggerVictimHauntedEvent(string audioClipName)
		{
			Plugin.Log.LogInfo($"Triggered victim haunted event");
			
			VictimHauntedEventData data = new VictimHauntedEventData(audioClipName);
			m_victimHauntedEvent.SendAllClients(data);
		}

		private void ReceiveVictimHauntedEvent(VictimHauntedEventData data, ulong fromUser)
		{
			PlayerControllerB localPlayerController = StartOfRound.Instance.localPlayerController;
			Plugin.Log.LogInfo($"Victim haunted received for user {fromUser}. The spectated user is {localPlayerController.actualClientId}");

			ulong specatedUserId = localPlayerController.spectatedPlayerScript.actualClientId;
			if (specatedUserId == fromUser) 
				m_ghostSfxPlayer.PlaySpectatorSfx(data.ClipName);
		}
	}
}