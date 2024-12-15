using HarmonyLib;
using UnityEngine;

namespace InternalModBot
{
    [HarmonyPatch(typeof(AnalyticsManager))]
    static class AnalyticsManager_Patch
    {
        [HarmonyPrefix]
        [HarmonyPatch(nameof(AnalyticsManager.SendDataToLoggly), new System.Type[] { typeof(WWWForm) })]
        static void SendDataToLoggly_Prefix(WWWForm form)
        {
            // Allow game developers to filter out crash logs on modded clients
            form.AddField("IsModdedClient", "true");
        }
    }
}