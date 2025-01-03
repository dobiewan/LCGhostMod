namespace DobieWan;

using System;
using UnityEngine;

[Serializable]
internal struct HauntVictimEventData
{
	[SerializeField]
	private ulong m_spectatedUserId;

	internal ulong SpectatedUserId
	{
		readonly get => m_spectatedUserId;
		private set => m_spectatedUserId = value;
	}

	internal HauntVictimEventData(ulong mSpectatedUserId)
	{
		SpectatedUserId = mSpectatedUserId;
	}
}