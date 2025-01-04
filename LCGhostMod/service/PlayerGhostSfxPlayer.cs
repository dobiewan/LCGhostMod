namespace DobieWan;

using config;
using UnityEngine;

internal class PlayerGhostSfxPlayer
{
	private readonly SfxPlayerConfigs m_configs = null;
	private readonly AudioSource m_audioSource = null;
	private readonly AudioClip[] m_ghostSfx = null;
	
	private int m_ghostSfxIndex = 0;

	internal PlayerGhostSfxPlayer()
	{
		m_configs = Plugin.Instance.SfxPlayerConfigs;
		
		AssetBundle bundle = Plugin.Instance.AssetBundle;
		if (bundle == null)
			return;

		// Get sfx from the bundle
		m_ghostSfx = bundle.LoadAllAssets<AudioClip>();
		m_ghostSfx.Shuffle();
		Plugin.Log.LogInfo($"Loaded {m_ghostSfx?.Length ?? 0} audio clips");

		// Try find audio source
		GameObject sfxParent = GameObject.Find("Systems/Audios/SFX");
		if (sfxParent == null)
		{
			Plugin.Log.LogError("Failed to find SFX object");
			return;
		}

		if (!sfxParent.TryGetComponent(out m_audioSource))
		{
			Plugin.Log.LogError("Failed to find SFX audio source");
			return;
		}

		Plugin.Log.LogInfo("PlayerGhostSfxPlayer successfully initialized!");
	}
  
	// TODO : group clips by voice so each player keeps a consistent voice?
	internal string PlaySfx()
	{
		if (m_ghostSfx == null || m_ghostSfx.Length == 0 || m_audioSource == null)
			return null;

		Plugin.Log.LogInfo("ooOOOOOOOoooooOOOoo");

		// TODO: set volume according to the amplitude of the voice input?
		AudioClip nextAudioClip = GetNextAudioClip();
		Utility.PlayAudioClipLocalOnly(m_audioSource, nextAudioClip, m_configs.RandomizePitchRange.Value, m_configs.RandomizeVolumeRange.Value);
		return nextAudioClip.name;
	}

	internal AudioClip GetNextAudioClip()
	{
		if (m_ghostSfxIndex >= m_ghostSfx.Length)
		{
			ReshuffleAudioClips();
			m_ghostSfxIndex = 0;
		}

		return m_ghostSfx[m_ghostSfxIndex++];
	}

	internal void ReshuffleAudioClips()
	{
		AudioClip previousClip = m_ghostSfx[^1];

		m_ghostSfx.Shuffle();

		// Make sure the first clip isn't the one we just played
		AudioClip firstClip = m_ghostSfx[0];
		if (m_ghostSfx.Length > 1 && previousClip == firstClip)
		{
			m_ghostSfx[0] = m_ghostSfx[^1];
			m_ghostSfx[^1] = firstClip;
		}
	}

	internal void PlaySpectatorSfx(string clipName)
	{
		if (!TryGetAudioClipByName(clipName, out AudioClip audioClip))
		{
			// Fall back to a random clip
			audioClip = m_ghostSfx[Random.Range(0, m_ghostSfx.Length)];
		}

		Utility.PlayAudioClipLocalOnly(m_audioSource, audioClip, m_configs.RandomizePitchRange.Value, m_configs.RandomizeVolumeRange.Value);
	}

	private bool TryGetAudioClipByName(string clipName, out AudioClip audioClip)
	{
		for (int i = 0; i < m_ghostSfx.Length; i++)
		{
			AudioClip clip = m_ghostSfx[i];
			if (clip != null && clip.name == clipName)
			{
				audioClip = clip;
				return true;
			}
		}

		audioClip = null;
		return false;
	}
}