using System.IO;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;

namespace Dobes;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
[BepInDependency("LethalNetworkAPI")]
public class Plugin : BaseUnityPlugin
{
    internal static Plugin Instance { get; private set; }
    internal static ManualLogSource Log => Instance.Logger;
    
    internal AssetBundle AssetBundle { get; private set; }
    
    private readonly Harmony _harmony = new(PluginInfo.PLUGIN_GUID);

    public Plugin()
    {
        Instance = this;
    }

    private void Awake()
    {
        Log.LogInfo($"LCGhostMod is awake!");
        Log.LogInfo($"Applying patches...");
        ApplyPluginPatch();
        Log.LogInfo($"Patches applied");

        TryLoadAssetBundle();
    }

    /// <summary>
    /// Applies the patch to the game.
    /// </summary>
    private void ApplyPluginPatch()
    {
        _harmony.PatchAll(typeof(RoundManagerPatch));
    }

    private void TryLoadAssetBundle()
    {
        string sAssemblyLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        if (sAssemblyLocation == null)
        {
            Log.LogError("Failed to get bundle directory.");
            return;
        }

        AssetBundle = AssetBundle.LoadFromFile(Path.Combine(sAssemblyLocation, "lcghostmodassets"));
        if (AssetBundle == null) 
        {
            Log.LogError("Failed to load custom assets.");
            return;
        }

        Log.LogInfo("LCGhostMod bundle loaded!");
    }
}
