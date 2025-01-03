namespace Dobes;

internal struct VictimHauntedEventData
{
    internal string ClipName { get; private set; }

    internal VictimHauntedEventData(string clipName)
    {
        ClipName = clipName;
    }
}