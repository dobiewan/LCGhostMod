using Dobes.structures;
using LethalNetworkAPI;

namespace Dobes;

internal static class LethalNetworkMessages
{
    internal static readonly LethalClientMessage<GhostEventData> s_ghostEventMessage = new LethalClientMessage<GhostEventData>(identifier: "GhostEvent");
    internal static readonly LethalClientMessage<string> s_testEventMessageClient = new LethalClientMessage<string>(identifier: "TestEvent");
    internal static readonly LethalClientMessage<string> s_testEventMessageServer = new LethalClientMessage<string>(identifier: "TestEvent");
    
    public static LethalClientMessage<string> customClientMessage = new("TestEvent2", onReceived: GhostManager.ReceiveFromServer, onReceivedFromClient: GhostManager.ReceiveFromClient);
    public static LethalServerMessage<string> customServerMessage = new("TestEvent2", onReceived: GhostManager.ReceiveFromClient);
}