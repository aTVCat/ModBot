using HarmonyLib;
using UnityEngine.UI;
using VR.UI;

namespace InternalModBot
{
    [HarmonyPatch(typeof(VersionText))]
    static class VersionText_Patch
    {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(VersionText.Start))]
        static void Start_Postfix(VersionText __instance)
        {
            string modBotVersionLabel = ModBotLocalizationManager.FormatLocalizedStringFromID("modbotversion", ModLibrary.Properties.Resources.ModBotVersion);
            __instance._text.text += $"\r\n{modBotVersionLabel}";
        }
    }
}