using System;
using Unity.Netcode;

namespace Dobes;

public class NetworkHandler : NetworkBehaviour
{
    internal static NetworkHandler Instance { get; private set; }
    
    public static event Action<String> LevelEvent;
    
    public override void OnNetworkSpawn()
    {
        LevelEvent = null;
        
        if (NetworkManager.Singleton.IsHost || NetworkManager.Singleton.IsServer)
            Instance?.gameObject.GetComponent<NetworkObject>().Despawn();
        
        Instance = this;

        base.OnNetworkSpawn();
    }

    [ClientRpc]
    public void EventClientRpc(string eventName)
    {
        LevelEvent?.Invoke(eventName);
    }
    
    // [ServerRpc(RequireOwnership = false)]
    // public void EventServerRPC(/*parameters here*/)
    // {
    //     // code here
    // }
}