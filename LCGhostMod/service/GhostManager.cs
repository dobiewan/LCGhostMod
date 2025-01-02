using Dobes.structures;

namespace Dobes
{
	using GameNetcodeStuff;
    using UnityEngine;

    /// <summary>
    /// 	
    /// </summary>
    /// <author>Sarah Dobie</author>
	internal class GhostManager : MonoBehaviour
	{
		private PlayerGhostEventDetector m_ghostEventDetector = null;
		private PlayerGhostSfxPlayer m_ghostSfxPlayer = null;

		private void Start()
		{
			m_ghostEventDetector = new PlayerGhostEventDetector(TriggerGhostEvent);
			m_ghostSfxPlayer = new PlayerGhostSfxPlayer();

			LethalNetworkMessages.s_ghostEventMessage.OnReceived += ReceiveGhostEvent;
			LethalNetworkMessages.s_testEventMessage.OnReceived += ReceiveTestEvent;
			
            Plugin.Log.LogInfo("GhostManager initialized!");
        }

		private void OnDestroy()
		{
			LethalNetworkMessages.s_ghostEventMessage.OnReceived -= ReceiveGhostEvent;
			LethalNetworkMessages.s_testEventMessage.OnReceived -= ReceiveTestEvent;
		}

		private void Update()
		{
			if (StartOfRound.Instance.localPlayerController.isCrouching)
			{
				Plugin.Log.LogInfo("Sending test event");
				LethalNetworkMessages.s_testEventMessage.SendAllClients("hello!");
			}
		}

		private void LateUpdate()
		{
			m_ghostEventDetector.Simulate();
		}

		private void ReceiveTestEvent(string obj)
		{
			Plugin.Log.LogInfo("Received test event: " + obj);
		}

		private void TriggerGhostEvent(PlayerControllerB forPlayer)
		{
            ulong forPlayerId = forPlayer.actualClientId;
			Plugin.Log.LogInfo($"Triggered ghost event for {forPlayerId}");
			
			GhostEventData data = new GhostEventData(forPlayerId);
			LethalNetworkMessages.s_ghostEventMessage.SendAllClients(data, false);
        }

		private void ReceiveGhostEvent(GhostEventData data)
		{
			PlayerControllerB localPlayerController = StartOfRound.Instance.localPlayerController;
			Plugin.Log.LogInfo($"Ghost event received for user {data.SpectatedUserId}. This user is {localPlayerController.actualClientId}");

			if (localPlayerController.actualClientId == data.SpectatedUserId) 
				m_ghostSfxPlayer.PlaySfx();
		}
	}
}