using HarmonyLib;
using Unity.Netcode;
using UnityEngine;

namespace Dobes;

[HarmonyPatch]
public class NetworkObjectManager
{
    static GameObject s_networkPrefab = null;

    [HarmonyPostfix, HarmonyPatch(typeof(GameNetworkManager), nameof(GameNetworkManager.Start))]
    public static void Init()
    {
        if (s_networkPrefab != null)
            return;
        
        s_networkPrefab = (GameObject)Plugin.Instance.AssetBundle.LoadAsset("NetworkHandler");
        s_networkPrefab.AddComponent<NetworkHandler>();
        
        NetworkManager.Singleton.AddNetworkPrefab(s_networkPrefab); 
    }
    
    [HarmonyPostfix, HarmonyPatch(typeof(StartOfRound), nameof(StartOfRound.Awake))]
    static void SpawnNetworkHandler()
    {
        if (NetworkManager.Singleton.IsHost || NetworkManager.Singleton.IsServer)
        {
            GameObject networkHandlerHost = Object.Instantiate(s_networkPrefab, Vector3.zero, Quaternion.identity);
            networkHandlerHost.GetComponent<NetworkObject>().Spawn();
        }
    }
}