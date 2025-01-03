namespace Dobes
{
    using HarmonyLib;
    using UnityEngine;

	[HarmonyPatch(typeof(RoundManager))]
	internal class RoundManagerPatch
	{
		[HarmonyPatch("Awake")]
		[HarmonyPostfix]
		static void RoundManagerAwake()
		{
			GameObject systemsParent = GameObject.Find("Systems");
			
			GameObject ghostManagerGo = new GameObject("GhostManager", typeof(EventManager));
			ghostManagerGo.transform.SetParent(systemsParent.transform);
        }
    }
}