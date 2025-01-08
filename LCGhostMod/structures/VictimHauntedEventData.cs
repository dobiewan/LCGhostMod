namespace DobieWan;

using System;
using UnityEngine;

[Serializable]
internal struct VictimHauntedEventData
{
	[SerializeField]
	private string m_clipName;

	[SerializeField] 
	private ulong m_fromUserId;
	
	internal string ClipName
	{
		readonly get => m_clipName;
		private set => m_clipName = value;
	}
	
	internal ulong FromUserId
	{
		readonly get => m_fromUserId;
		private set => m_fromUserId = value;
	}

	internal VictimHauntedEventData(string clipName, ulong fromUserId)
	{
		ClipName = clipName;
		FromUserId = fromUserId;
	}
}