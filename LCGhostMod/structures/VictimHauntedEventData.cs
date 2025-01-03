namespace Dobes;

using System;
using UnityEngine;

[Serializable]
internal struct VictimHauntedEventData
{
	[SerializeField]
	private string m_clipName;
	
	internal string ClipName
	{
		readonly get => m_clipName;
		private set => m_clipName = value;
	}

	internal VictimHauntedEventData(string clipName)
	{
		ClipName = clipName;
	}
}