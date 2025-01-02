using Dobes.structures;
using LethalNetworkAPI;

namespace Dobes;

internal static class LethalNetworkMessages
{
    internal static readonly LethalClientMessage<GhostEventData> s_ghostEventMessage = new LethalClientMessage<GhostEventData>(identifier: "GhostEvent");
}