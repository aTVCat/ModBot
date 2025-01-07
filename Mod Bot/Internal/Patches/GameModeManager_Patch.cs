using HarmonyLib;
using UnityEngine;

namespace InternalModBot
{
    [HarmonyPatch(typeof(GameModeManager))]
    static class GameModeManager_Patch
    {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(GameModeManager.ShouldStartVRGameFromTitleScreen))]
        static void ShouldStartVRGameFromTitleScreen_Postfix(ref bool __result)
        {
            if (!__result && !WorkshopLevelManager.Instance.HasAfterSceneLoadPlaytest())
            {
                __result = true;
            }
        }
    }
}