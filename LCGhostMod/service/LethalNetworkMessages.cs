using Dobes.structures;
using LethalNetworkAPI;

namespace Dobes;

internal static class LethalNetworkMessages
{
    internal static readonly LethalClientMessage<GhostEventData> s_ghostEventMessage = new LethalClientMessage<GhostEventData>(identifier: "GhostEvent");
    internal static readonly LethalClientMessage<string> s_testEventMessage = new LethalClientMessage<string>(identifier: "TestEvent");
}