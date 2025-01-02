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

			// TODO
            // LC_API.Networking.Network.RegisterMessage<string>(MESSAGE_ID, true, ReceiveGhostEvent);
        }

		private void LateUpdate()
		{
			m_ghostEventDetector.Simulate();
		}

		private void TriggerGhostEvent(PlayerControllerB forPlayer)
		{
            ulong forPlayerId = forPlayer.actualClientId;
			Plugin.Log.LogInfo($"Triggered ghost event for {forPlayerId}");
			
			// TODO
            // LC_API.Networking.Network.Broadcast(MESSAGE_ID, forPlayerId.ToString());
        }
		
		private void ReceiveGhostEvent(ulong arg1, string forPlayerId) // sure wish i knew what arg1 was
		{
			Plugin.Log.LogInfo($"Received ghost event for {forPlayerId}");
			PlayerControllerB localPlayerController = StartOfRound.Instance.localPlayerController;
			if (localPlayerController.actualClientId == ulong.Parse(forPlayerId)) 
				m_ghostSfxPlayer.PlaySfx();
		}
	}
}