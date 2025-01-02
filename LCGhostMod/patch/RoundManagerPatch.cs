namespace Dobes
{
    using HarmonyLib;
    using UnityEngine;

    /// <summary>
    /// 	
    /// </summary>
    /// <author>Sarah Dobie</author>
	[HarmonyPatch(typeof(RoundManager))]
	internal class RoundManagerPatch
	{
		[HarmonyPatch("Awake")]
		[HarmonyPostfix]
		static void RoundManagerAwake()
		{
			GameObject systemsParent = GameObject.Find("Systems");
			
			GameObject ghostManagerGo = new GameObject("GhostManager", typeof(GhostManager));
			ghostManagerGo.transform.SetParent(systemsParent.transform);
        }
    }
}