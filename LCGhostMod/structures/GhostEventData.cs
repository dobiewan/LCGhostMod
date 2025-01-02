using System;

namespace Dobes.structures;

internal struct GhostEventData
{
    internal ulong SpectatedUserId { get; private set; }

    internal GhostEventData(ulong mSpectatedUserId)
    {
        SpectatedUserId = mSpectatedUserId;
    }

    public override string ToString()
    {
        string s = "";
        
        s += $"SpectatedUserId: {SpectatedUserId}";
        
        return s;
    }

    public static bool TryParse(string dataString, out GhostEventData ghostEventData)
    {
        ghostEventData = default;

        try
        {
            string[] lines = dataString.Split('\n');
        
            if (!lines[0].StartsWith("SpectatedUserId: ")) return false;
            ghostEventData.SpectatedUserId = ulong.Parse(lines[0]);
            return true;
        }
        catch (Exception e)
        {
            Plugin.Log.LogError("Failed to parse ghost event due to an exception: " + e.Message);
            return false;
        }
    }
}