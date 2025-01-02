namespace Dobes.structures;

internal struct GhostEventData
{
    internal ulong SpectatedUserId { get; private set; }
    // TODO more stuff here

    internal GhostEventData(ulong mSpectatedUserId)
    {
        SpectatedUserId = mSpectatedUserId;
    }
}