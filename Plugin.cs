using BepInEx;

namespace KeepShipUpgrades
{
    [BepInPlugin("pop.mods.keepshipupgrades", "KeepShipUpgrades", "0.0.1")]
    public class Plugin : BaseUnityPlugin
    {
        private void Awake()
        {
            // Plugin startup logic
            Logger.LogInfo($"KeepShipUpgrades has loaded :)");
        }
    }
}
