namespace Dobes;

internal struct HauntVictimEventData
{
	internal ulong SpectatedUserId { get; private set; }
	// TODO more stuff here

	internal HauntVictimEventData(ulong mSpectatedUserId)
	{
		SpectatedUserId = mSpectatedUserId;
	}
}