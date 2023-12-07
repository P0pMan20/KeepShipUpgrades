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
            Harmony.CreateAndPatchAll(typeof(EnsureShipUpgradesAreKept));
        }
    }
}

class EnsureShipUpgradesAreKept
{
    // this is really shit and should be rewritten to be more easily updatable
    // and less copyright infringing
    // and less fuck up others mods
    // it literally just copies the original code (from ILSpy) and removes all the shit that deletes upgrades
    // We also skip any prefix that comes after this mod
    // ideally you would copy the list of bought unlockables (ie loud horn, tv and pumpkin) before removal
    // and give them back in a postfix
    // might do it tomorrow
    [HarmonyPatch(typeof(StartOfRound), "ResetShip")]
    [HarmonyPrefix]
    static bool SaveUpgrades(ref StartOfRound __instance)
    {
        TimeOfDay.Instance.globalTime = 100f;
        TimeOfDay.Instance.profitQuota = TimeOfDay.Instance.quotaVariables.startingQuota;
        TimeOfDay.Instance.quotaFulfilled = 0;
        TimeOfDay.Instance.timesFulfilledQuota = 0;
        TimeOfDay.Instance.timeUntilDeadline = (int)(TimeOfDay.Instance.totalTime * (float)TimeOfDay.Instance.quotaVariables.deadlineDaysAmount);
        TimeOfDay.Instance.UpdateProfitQuotaCurrentTime();
        __instance.randomMapSeed++;
        Debug.Log("Reset ship 0");
        __instance.companyBuyingRate = 0.3f;
        __instance.ChangeLevel(__instance.defaultPlanet);
        __instance.ChangePlanet();
        __instance.SetMapScreenInfoToCurrentLevel();
        Terminal terminal = Object.FindObjectOfType<Terminal>();
        if (terminal != null)
        {
            terminal.groupCredits = TimeOfDay.Instance.quotaVariables.startingCredits;
        }
        if (__instance.IsServer)
        {
            RoundManager.Instance.DespawnPropsAtEndOfRound(despawnAllItems: true);
            __instance.closetLeftDoor.SetBoolOnClientOnly(setTo: false);
            __instance.closetRightDoor.SetBoolOnClientOnly(setTo: false);
        }
        Debug.Log("Reset ship A");
        for (int l = 0; l < __instance.allPlayerScripts.Length; l++)
        {
            SoundManager.Instance.playerVoicePitchTargets[l] = 1f;
            __instance.allPlayerScripts[l].ResetPlayerBloodObjects();
            UnlockableSuit.SwitchSuitForPlayer(__instance.allPlayerScripts[l], 0);
        }
        Debug.Log("Reset ship D");
        TimeOfDay.Instance.OnDayChanged();
        // foreach (var locker in __instance.SpawnedShipUnlockables)
        // {
        //     Debug.Log();
        // }
        //
        //
        // foreach (var item in __instance.unlockablesList.unlockables)
        // {
        //     foreach (var field in item.GetType().GetFields())
        //     {
        //         Debug.Log($"{field.Name} F {field.GetValue(item)}");
        //     }
        // }
        return false;
    }
    
    // // skip reset
    // [HarmonyPatch(typeof(GameNetworkManager), "ResetUnlockablesListValues")]
    // [HarmonyPrefix]
    // static bool GameNoReset(ref GameNetworkManager __instance)
    // {
    //     return true;
    // }    
    // [HarmonyPatch(typeof(RoundManager), "DespawnPropsAtEndOfRound")]
    // [HarmonyPrefix]
    // static bool NoDeez(ref RoundManager __instance)
    // {
    //     return true;
    // }
}