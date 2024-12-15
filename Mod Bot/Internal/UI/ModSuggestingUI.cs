using ICSharpCode.SharpZipLib.Zip;
using ModLibrary;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace InternalModBot
{
    /// <summary>
    /// Used by Mod-Bot to control the Twich mode mod suggesting
    /// </summary>
    internal class ModSuggestingUI : MonoBehaviour
    {
        /// <summary>
        /// The animator that plays the slide in and out animation
        /// </summary>
        public Animator ModSuggestionAnimator;

        /// <summary>
        /// Text text display where all info will be displayed
        /// </summary>
        public Text DisplayText;

        /// <summary>
        /// The mod ids of the mods the user has already rejected, static to persist across scene swiches
        /// </summary>
        public static HashSet<string> DeniedModIds = new HashSet<string>();

        /// <summary>
        /// Sets up the mod suggesting ui from a modded object
        /// </summary>
        /// <param name="moddedObject"></param>
        public void Init(ModdedObject moddedObject)
        {
            DisplayText = moddedObject.GetObject<Text>(0);
            ModSuggestionAnimator = moddedObject.GetObject<Animator>(1);
        }
        void Start()
        {
            GlobalEventManager.Instance.AddEventListener(GlobalEvents.LevelSpawned, ShowNextInSuggestedModsQueue);
        }

        void OnDestroy()
        {
            GlobalEventManager.Instance.RemoveEventListener(GlobalEvents.LevelSpawned, ShowNextInSuggestedModsQueue);
        }

        /// <summary>
        /// Shows the next in the suggested mods queue
        /// </summary>
        public void ShowNextInSuggestedModsQueue()
        {
        }

        /// <summary>
        /// Brings up the suggest window for a multiplayer suggested mod
        /// </summary>
        /// <param name="suggesterPlayfabID"></param>
        /// <param name="modId"></param>
        public void SuggestModMultiplayer(string suggesterPlayfabID, string modId)
        {
        }

        Queue<ModSuggestion> _modSuggestionQueue = new Queue<ModSuggestion>();

        struct ModSuggestion
        {
            public ModSuggestion(string modName, string modCreator, string suggesterName, string modID)
            {
                ModName = modName;
                ModCreator = modCreator;
                SuggesterName = suggesterName;
                ModID = modID;
            }

            public string ModName;
            public string ModCreator;
            public string SuggesterName;
            public string ModID;

        }

    }

}
