using BepInEx;
using HarmonyLib;
using UnityEngine;

namespace KeepShipUpgrades
{
    [BepInPlugin("pop.mods.keepshipupgrades", "KeepShipUpgrades", "0.0.1")]
    public class Plugin : BaseUnityPlugin
    {
        // Cheers floop for the idea
        private void Awake()
        {
            // Plugin startup logic
            Logger.LogInfo($"KeepShipUpgrades has loaded :)");
        }
    }
}

class EnsureShipUpgradesAreKept
{
    [HarmonyPatch("StartOfRound", "ResetShip")]
    [HarmonyPrefix]
    static void SaveUpgrades(ref StartOfRound __instance)
    {
        foreach (var item in __instance.unlockablesList.unlockables)
        {
            foreach (var prop in item.GetType().GetProperties())
            {
                Debug.Log($"{prop.Name} P {prop.GetValue(item)}");
            }
            foreach (var field in item.GetType().GetFields())
            {
                Debug.Log($"{field.Name} F {field.GetValue(item)}");
            }
        }

    }
    
    // skip reset
    [HarmonyPatch("GameNetworkManager", "ResetUnlockablesListValues")]
    [HarmonyPrefix]
    static bool GameNoReset(ref GameNetworkManager __instance)
    {
        return false;
    }
}