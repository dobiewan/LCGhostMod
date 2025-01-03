namespace DobieWan;

using System;
using config;
using Dissonance;
using GameNetcodeStuff;
using UnityEngine;
using Random = UnityEngine.Random;

internal class PlayerGhostEventDetector
{
	private readonly EventConfigs m_configs = null;
	private readonly Action<PlayerControllerB> m_hauntVictimEventAction = null;

	private float m_lastGhostNoiseTime = 0f;
	private float m_cooldownTime = 0f;

	internal PlayerGhostEventDetector(Action<PlayerControllerB> hauntVictimEventAction)
	{
		m_configs = Plugin.Instance.EventConfigs;
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
		if (voicePlayerState.Amplitude < m_configs.MinMicAmpToTriggerGhost.Value)
			return;

		TriggerGhostEvent(localPlayer.spectatedPlayerScript);
	}

	private void TriggerGhostEvent(PlayerControllerB toPlayer)
	{
		m_lastGhostNoiseTime = Time.time;
		m_cooldownTime = GetRandomCooldownTime();
		m_hauntVictimEventAction?.Invoke(toPlayer);
	}

	private float GetRandomCooldownTime()
	{
		float minCooldownTime = m_configs.EventTriggerCooldownMin.Value;
		float maxCooldownTime = m_configs.EventTriggerCooldownMax.Value;
		return minCooldownTime + Random.value * (maxCooldownTime - minCooldownTime);
	}
}