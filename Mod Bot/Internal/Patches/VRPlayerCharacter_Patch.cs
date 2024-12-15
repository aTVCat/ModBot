using HarmonyLib;
using UnityEngine.UI;

namespace InternalModBot
{
    [HarmonyPatch(typeof(VRPlayerCharacter))]
    static class VRPlayerCharacter_Patch
    {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(VRPlayerCharacter.Start))]
        static void Start_Postfix(VRPlayerCharacter __instance)
        {
            __instance.gameObject.AddComponent<KeyboardMousePlayerController>();
        }
    }
}