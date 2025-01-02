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
		// private const string MESSAGE_ID = "GhostMessage";

		private PlayerGhostEventDetector m_ghostEventDetector = null;
		private PlayerGhostSfxPlayer m_ghostSfxPlayer = null;

		private void Start()
		{
			m_ghostEventDetector = new PlayerGhostEventDetector(TriggerGhostEvent);
			m_ghostSfxPlayer = new PlayerGhostSfxPlayer();

			NetworkHandler.LevelEvent += ReceiveGhostEvent;
			
            Plugin.Log.LogInfo("GhostManager initialized!");
        }

		private void OnDestroy()
		{
			NetworkHandler.LevelEvent -= ReceiveGhostEvent;
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
			SendGhostEventToClients(data);
        }

		static void SendGhostEventToClients(GhostEventData data)
		{
			// if (!(NetworkManager.Singleton.IsHost || NetworkManager.Singleton.IsServer))
			// 	return;

			NetworkHandler.Instance.EventClientRpc(data.ToString());
		}

		private void ReceiveGhostEvent(string dataString)
		{
			if (!GhostEventData.TryParse(dataString, out GhostEventData data))
			{
				Plugin.Log.LogError("Failed to parse ghost event data");
				return;
			}
			
			PlayerControllerB localPlayerController = StartOfRound.Instance.localPlayerController;
			if (localPlayerController.actualClientId == data.SpectatedUserId) 
				m_ghostSfxPlayer.PlaySfx();
		}
	}
}