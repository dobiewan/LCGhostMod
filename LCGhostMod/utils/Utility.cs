using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DobieWan;

internal static class Utility
{
	internal static void Shuffle<T>(this IList<T> list)
	{
		for (int i = 0; i < list.Count; i++)
		{
			T tmp = list[i];
			int r = Random.Range(i, list.Count);
			list[i] = list[r];
			list[r] = tmp;
		}
	}

	// Linq is inefficient and I will never use it
	internal static bool Contains<T>(this IList<T> list, T value) where T : IEquatable<T>
	{
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i].Equals(value))
			{
				return true;
			}
		}

		return false;
	}

	internal static void PlayAudioClipLocalOnly(
		AudioSource audioSource,
		AudioClip clip,
		float randomizePitchRange = 0f,
		float randomizeVolumeRange = 0f,
		float volume = 1f)
	{
		if (randomizePitchRange > 0f)
			audioSource.pitch = Random.Range(1f - randomizePitchRange, 1f + randomizePitchRange);

		float volumeScale = 1f;
		if (randomizeVolumeRange > 0f)
			volumeScale = Random.Range(volume - randomizeVolumeRange, volume + randomizeVolumeRange);

		audioSource.PlayOneShot(clip, volumeScale);
	}
}