namespace DobieWan;

using UnityEngine;

internal class GhostSfxPlayer
{
	private AudioSource AudioSource
	{
		get
		{
			if (m_audioSource == null)
			{
				// Try find audio source
				GameObject sfxParent = GameObject.Find("Systems/Audios/SFX");
				if (sfxParent == null)
				{
					Plugin.Log.LogError("Failed to find SFX object");
					return null;
				}

				if (!sfxParent.TryGetComponent(out m_audioSource))
				{
					Plugin.Log.LogError("Failed to find SFX audio source");
					return null;
				}
			}

			return m_audioSource;
		}
	}

	private readonly SfxPlayerConfigs m_configs = null;
	private readonly AudioClip[] m_ghostSfx = null;
	
	private AudioSource m_audioSource = null;
	
	private int m_ghostSfxIndex = 0;

	internal GhostSfxPlayer()
	{
		m_configs = Plugin.Instance.SfxPlayerConfigs;
		
		AssetBundle bundle = Plugin.Instance.AssetBundle;
		if (bundle == null)
			return;

		// Get sfx from the bundle
		m_ghostSfx = bundle.LoadAllAssets<AudioClip>();
		m_ghostSfx.Shuffle();
		Plugin.Log.LogInfo($"Loaded {m_ghostSfx?.Length ?? 0} audio clips");
	}
  
#region Victim
	// TODO : group clips by voice so each player keeps a consistent voice?
	internal string PlayHauntedSfx()
	{
		if (m_ghostSfx == null || m_ghostSfx.Length == 0 || AudioSource == null)
			return null;

		// TODO: set volume according to the amplitude of the voice input?
		AudioClip nextAudioClip = GetNextAudioClip();
		Utility.PlayAudioClipLocalOnly(AudioSource, nextAudioClip, m_configs.RandomizePitchRange.Value, m_configs.RandomizeVolumeRange.Value);
		Plugin.Log.LogInfo("ooOOOOOOOoooooOOOoo");
		
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
#endregion

#region Spectator
	internal void PlaySpectatorSfx(string clipName)
	{
		if (!TryGetAudioClipByName(clipName, out AudioClip audioClip))
		{
			// Fall back to a random clip
			audioClip = m_ghostSfx[Random.Range(0, m_ghostSfx.Length)];
		}

		Utility.PlayAudioClipLocalOnly(AudioSource, audioClip, m_configs.RandomizePitchRange.Value, m_configs.RandomizeVolumeRange.Value);
		Plugin.Log.LogInfo("ooOOOOOOOoooooOOOoo");
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
#endregion
}