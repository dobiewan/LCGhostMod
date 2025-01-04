namespace DobieWan.config;

using BepInEx.Configuration;

internal class SfxPlayerConfigs : IConfigs
{
	private const string GROUP_NAME = "SFX";

	internal ConfigEntry<float> RandomizePitchRange { get; private set; }
	internal ConfigEntry<float> RandomizeVolumeRange { get; private set; }

	void IConfigs.Initialize(ConfigFile config)
	{
		RandomizePitchRange = config.Bind(GROUP_NAME, "Randomize Pitch Range", 0.2f, "Bigger number means ghost SFX pitch can be randomized further from normal.");
		RandomizeVolumeRange = config.Bind(GROUP_NAME, "Randomize Volume Range", 0.2f, "Bigger number means ghost SFX volume can be randomized further from normal.");
	}
}