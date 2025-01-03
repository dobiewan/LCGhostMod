using LethalNetworkAPI;

namespace Dobes
{
	using GameNetcodeStuff;
    using UnityEngine;

	internal class EventManager : MonoBehaviour
	{
		private LethalClientMessage<GhostEventData> m_ghostEventMessage = null;
		
		private PlayerGhostEventDetector m_ghostEventDetector = null;
		private PlayerGhostSfxPlayer m_ghostSfxPlayer = null;

		private void Start()
		{
			m_ghostEventDetector = new PlayerGhostEventDetector(TriggerGhostEvent);
			m_ghostSfxPlayer = new PlayerGhostSfxPlayer();
			
			m_ghostEventMessage = new LethalClientMessage<GhostEventData>("GhostEvent", onReceivedFromClient: ReceiveGhostEvent);
			
            Plugin.Log.LogInfo("GhostManager initialized!");
        }

		private void LateUpdate()
		{
			m_ghostEventDetector.Simulate();
		}

		private void TriggerGhostEvent(PlayerControllerB forPlayer)
		{
            ulong forPlayerId = forPlayer.actualClientId;
			Plugin.Log.LogInfo($"Triggered ghost event for {forPlayerId}");
			
			GhostEventData data = new GhostEventData(forPlayerId);
			m_ghostEventMessage.SendAllClients(data, false);
        }

		private void ReceiveGhostEvent(GhostEventData data, ulong fromUser)
		{
			PlayerControllerB localPlayerController = StartOfRound.Instance.localPlayerController;
			Plugin.Log.LogInfo($"Ghost event received for user {data.SpectatedUserId}. This user is {localPlayerController.actualClientId}");

			if (localPlayerController.actualClientId == data.SpectatedUserId) 
				m_ghostSfxPlayer.PlaySfx();
		}
	}
}