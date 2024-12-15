using HarmonyLib;

namespace InternalModBot
{
    [HarmonyPatch(typeof(EscMenu))]
    static class EscMenu_Patch
    {
        [HarmonyPrefix]
        [HarmonyPatch(nameof(LevelEditorUI.Show))]
        static bool Show_Prefix()
        {
            return GameModeManager.IsInLevelEditor();
        }
    }
}
