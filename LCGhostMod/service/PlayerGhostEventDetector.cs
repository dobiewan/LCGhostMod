namespace Dobes
{
	using System;
	using Dissonance;
	using GameNetcodeStuff;
	using UnityEngine;
	using Random = UnityEngine.Random;

	internal class PlayerGhostEventDetector
	{
		// TODO DOBIE: put this stuff in a conf file
		private const float COOLDOWN_MIN = 2f;
		private const float COOLDOWN_MAX = 5f;
		private const float MIN_AMPLITUDE_FOR_SFX = 0.4f;
		
		private readonly Action<PlayerControllerB> m_hauntVictimEventAction = null;
		private float m_lastGhostNoiseTime = 0f;
		private float m_cooldownTime = 0f;

		internal PlayerGhostEventDetector(Action<PlayerControllerB> hauntVictimEventAction)
		{
			m_hauntVictimEventAction = hauntVictimEventAction;
			m_cooldownTime = GetRandomCooldownTime();
		}

		internal void Simulate()
		{
			StartOfRound startOfRound = StartOfRound.Instance;
			PlayerControllerB localPlayer = startOfRound.localPlayerController;
			DissonanceComms voiceChatModule = startOfRound.voiceChatModule;
            
			if (!localPlayer.isPlayerDead
				|| m_lastGhostNoiseTime + m_cooldownTime > Time.time
				|| voiceChatModule == null
				|| localPlayer.spectatedPlayerScript == null
			)
				return;

			string localPlayerName = voiceChatModule.LocalPlayerName;
			if (string.IsNullOrEmpty(localPlayerName))
				return;
			
			VoicePlayerState voicePlayerState = voiceChatModule.FindPlayer(voiceChatModule.LocalPlayerName);
			
			if (voicePlayerState == null || !voicePlayerState.IsSpeaking)
				return;

            // Plugin.Log.LogInfo("local mic amplitude: " + voicePlayerState.Amplitude);

            // TODO DOBIE: make the min amp configurable by user
			if (voicePlayerState.Amplitude < MIN_AMPLITUDE_FOR_SFX)
				return;
            
			TriggerGhostEvent(localPlayer.spectatedPlayerScript);
		}

		private void TriggerGhostEvent(PlayerControllerB toPlayer)
		{
			m_lastGhostNoiseTime = Time.time;
			m_cooldownTime = GetRandomCooldownTime();
			m_hauntVictimEventAction?.Invoke(toPlayer);
		}

		private static float GetRandomCooldownTime()
		{
			return COOLDOWN_MIN + Random.value * (COOLDOWN_MAX - COOLDOWN_MIN);
		}
	}
}