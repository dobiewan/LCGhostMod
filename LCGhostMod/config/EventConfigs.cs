namespace DobieWan.config;

using BepInEx.Configuration;

internal class EventConfigs : IConfigs
{
	private const string GROUP_NAME = "GHOST EVENT";

	internal ConfigEntry<float> MinMicAmpToTriggerGhost { get; private set; }
	internal ConfigEntry<float> EventTriggerCooldownMin { get; private set; }
	internal ConfigEntry<float> EventTriggerCooldownMax { get; private set; }

	void IConfigs.Initialize(ConfigFile config)
	{
		MinMicAmpToTriggerGhost = config.Bind(GROUP_NAME, "Microphone Amp Threshold", 0.4f, "This is the threshold amplitude for your microphone input to trigger a ghost event. A higher value means you must speak louder to trigger the event. Recommended between 0.1 and 2.0.");
		EventTriggerCooldownMin = config.Bind(GROUP_NAME, "Min Cooldown Time", 2f, "Minimum length of time between ghost events triggered by you.");
		EventTriggerCooldownMax = config.Bind(GROUP_NAME, "Max Cooldown Time", 5f, "Maximum length of time between ghost events triggered by you.");
	}
}