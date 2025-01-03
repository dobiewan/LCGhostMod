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
			LethalNetworkMessages.s_testEventMessageServer.OnReceived += ReceiveTestEventServer;
			LethalNetworkMessages.s_testEventMessageClient.OnReceived += ReceiveTestEventClient;
			
            Plugin.Log.LogInfo("GhostManager initialized!");
        }

		private void OnDestroy()
		{
			LethalNetworkMessages.s_ghostEventMessage.OnReceived -= ReceiveGhostEvent;
			LethalNetworkMessages.s_testEventMessageServer.OnReceived -= ReceiveTestEventServer;
			LethalNetworkMessages.s_testEventMessageClient.OnReceived -= ReceiveTestEventClient;
		}
		
		private float m_timeOfLastTestEvent = 0f;

		private void Update()
		{
			if (StartOfRound.Instance.localPlayerController.isCrouching && Time.time - m_timeOfLastTestEvent >= 5f)
			{
				Plugin.Log.LogInfo("Sending test event");
				LethalNetworkMessages.s_testEventMessageClient.SendServer("hello client!");
				LethalNetworkMessages.s_testEventMessageServer.SendServer("hello server!");
				LethalNetworkMessages.customClientMessage.SendServer("hello server!");
				LethalNetworkMessages.customClientMessage.SendAllClients("hello all clients!");
				LethalNetworkMessages.customServerMessage.SendAllClients("hello all clients!");
			}
		}

		private void LateUpdate()
		{
			m_ghostEventDetector.Simulate();
		}

		private void ReceiveTestEventServer(string obj)
		{
			Plugin.Log.LogInfo("Received server test event: " + obj);
		}

		private void ReceiveTestEventClient(string obj)
		{
			Plugin.Log.LogInfo("Received client test event: " + obj);
		}

		public static void ReceiveFromServer(string obj)
		{
			Plugin.Log.LogInfo("Received server test event 2: " + obj);
		}

		public static void ReceiveFromClient(string obj, ulong id)
		{
			Plugin.Log.LogInfo("Received client test event 2: " + obj);
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