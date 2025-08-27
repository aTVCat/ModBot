using HarmonyLib;
using UnityEngine;

namespace InternalModBot
{
    [HarmonyPatch(typeof(DebugLaptop))]
    static class DebugLaptop_Patch
    {
        [HarmonyPrefix]
        [HarmonyPatch(nameof(DebugLaptop.Start))]
        static void Start_Prefix(DebugLaptop __instance)
        {
            __instance.Programs.Add(new DebugLaptopProgramInfo
            {
                Name = "Mods console",
                Color = new Color(1f, 0.4f, 0f, 1f),
                Controller = ModsPanelManager.Instance.GetDebugLaptopModsConsoleProgram()
            });
        }
    }
}
