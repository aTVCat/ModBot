using HarmonyLib;
using UnityEngine.UI;
using VR.UI;

namespace InternalModBot
{
    [HarmonyPatch(typeof(MainMenuUI))]
    static class MainMenuUI_Patch
    {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(MainMenuUI.Start))]
        static void Start_Postfix(MainMenuUI __instance)
        {
            if (ModsPanelManager.Instance)
                ModsPanelManager.Instance.PatchVRMainMenu(__instance);
        }
    }
}