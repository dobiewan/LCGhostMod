using System.IO;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;

namespace Dobes;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    internal static Plugin Instance { get; set; }
    internal AssetBundle AssetBundle => m_assetBundle;

    internal static ManualLogSource Log => Instance.Logger;

    private readonly Harmony _harmony = new(PluginInfo.PLUGIN_GUID);

    // public TemplateService Service;
    
    private AssetBundle m_assetBundle = null;

    public Plugin()
    {
        Instance = this;
    }

    private void Awake()
    {
        // Service = new TemplateService();

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

        m_assetBundle = AssetBundle.LoadFromFile(Path.Combine(sAssemblyLocation, "lcghostmod"));
        if (m_assetBundle == null) 
        {
            Log.LogError("Failed to load custom assets.");
            return;
        }

        Log.LogInfo("LCGhostModMain successfully initialized!");
    }
}
