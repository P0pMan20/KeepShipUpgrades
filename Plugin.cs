using System.Collections.Generic;
using System.Net.Http.Headers;
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
            Logger.LogInfo($"KeepShipUpgrades is finished patching :D");

        }
    }
}

class EnsureShipUpgradesAreKept
{
    private static List<UnlockableItem> backupList;
    private static int[] backupUnlockedShipObjects;

    [HarmonyPatch(typeof(GameNetworkManager), "ResetSavedGameValues")]
    [HarmonyPrefix]
    static void SaveUpgradesBeforeTheyGetDeleted(ref GameNetworkManager __instance)
    {
        backupList = StartOfRound.Instance.unlockablesList.unlockables;
        backupUnlockedShipObjects = ES3.Load<int[]>("UnlockedShipObjects", GameNetworkManager.Instance.currentSaveFileName);

    }
    
    
    // hopefully this all syncs across from the server to the clients
    [HarmonyPatch(typeof(StartOfRound), "EndPlayersFiredSequenceClientRpc")]
    [HarmonyPrefix]
    static void ReinstateUpgrades(ref StartOfRound __instance)
    {
        ES3.Save<int[]>("UnlockedShipObjects", backupUnlockedShipObjects, GameNetworkManager.Instance.currentSaveFileName);
        __instance.unlockablesList.unlockables = backupList;
        Debug.Log(backupList==__instance.unlockablesList.unlockables);
        Debug.Log(backupList==StartOfRound.Instance.unlockablesList.unlockables);
        for (int i = 0; i < __instance.unlockablesList.unlockables.Count; i++)
        {
            // I believe this is if moveable item and the other type is probably the suits
            // yes see StartOfRound.SpawnUnlockable https://discord.com/channels/926749935263698964/926749935263698967/1182945048254480384
            if (__instance.unlockablesList.unlockables[i].unlockableType == 1)
            {
                string itemName = __instance.unlockablesList.unlockables[i].unlockableName;
                ES3.Save("ShipUnlockMoved_"  + itemName , __instance.unlockablesList.unlockables[i].hasBeenMoved, GameNetworkManager.Instance.currentSaveFileName);
                ES3.Save("ShipUnlockStored_" + itemName, __instance.unlockablesList.unlockables[i].inStorage, GameNetworkManager.Instance.currentSaveFileName);
                ES3.Save("ShipUnlockPos_" + itemName, __instance.unlockablesList.unlockables[i].placedPosition, GameNetworkManager.Instance.currentSaveFileName);
                ES3.Save("ShipUnlockRot_" + itemName, __instance.unlockablesList.unlockables[i].placedRotation, GameNetworkManager.Instance.currentSaveFileName);
            }
        }
    }
    


}