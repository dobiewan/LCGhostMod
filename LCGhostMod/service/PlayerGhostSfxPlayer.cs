namespace Dobes
{
	using UnityEngine;

	internal class PlayerGhostSfxPlayer
	{
		private const float RANDOMIZE_PITCH_RANGE = 0.1f;
		private const float RANDOMIZE_VOLUME_RANGE = 0.2f;
		
		private readonly AudioSource m_audioSource = null;
		private readonly AudioClip[] m_ghostSfx = null;
		private int m_ghostSfxIndex = 0;

		internal PlayerGhostSfxPlayer()
		{
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

		internal void PlaySfx()
		{
			if (m_ghostSfx == null || m_ghostSfx.Length == 0 || m_audioSource == null)
				return;
			
			Plugin.Log.LogInfo("ooOOOOOOOoooooOOOoo");
			
			// TODO: set volume according to the amplitude of the voice input?
			Utility.PlayAudioClipLocalOnly(m_audioSource, GetNextAudioClip(), RANDOMIZE_PITCH_RANGE, RANDOMIZE_VOLUME_RANGE);
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
	}
}