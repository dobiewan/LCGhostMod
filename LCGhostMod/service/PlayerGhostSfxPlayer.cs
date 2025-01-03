namespace Dobes
{
	using UnityEngine;

	internal class PlayerGhostSfxPlayer
	{
		private readonly AudioSource m_audioSource = null;
		private readonly AudioClip[] m_ghostSfx = null;

		internal PlayerGhostSfxPlayer()
		{
			AssetBundle bundle = Plugin.Instance.AssetBundle;
			if (bundle == null)
				return;

			// Get sfx from the bundle
			m_ghostSfx = bundle.LoadAllAssets<AudioClip>();
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

		// TODO : group clips by voice so each player keeps a consistent voice
		// TODO : should the sender also hear the sfx?
		internal void PlaySfx()
		{
			if (m_ghostSfx == null || m_audioSource == null)
				return;
			
			Plugin.Log.LogInfo("ooOOOOOOOoooooOOOoo");
			RoundManager.PlayRandomClip(m_audioSource, m_ghostSfx, audibleNoiseID: -1); // TODO: set volume according to the amplitude of the voice input
		}
	}
}