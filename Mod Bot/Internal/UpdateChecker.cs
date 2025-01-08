using ModLibrary;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace InternalModBot
{
    /// <summary>
    /// Used by Mod-Bot to check if there is a newer version available
    /// </summary>
    internal class UpdateChecker : MonoBehaviour
    {
        void Start()
        {
            StartCoroutine(checkVersion()); // Needs to be a Coroutine since the web requests are not asynchronous
        }

        IEnumerator checkVersion()
        {
            yield return null;

            if (!isOnTitleScreen()) // If the user is currently playing any game mode, dont check for updates
                yield break;

            string installedModBotVersion = ModLibrary.Properties.Resources.ModBotVersion;
            if (installedModBotVersion.ToLower().Contains("beta"))
                yield break;

            using (UnityWebRequest modBotVersionRequest = UnityWebRequest.Get("https://modbot.org/api?operation=getCurrentModBotVersion"))
            {
                yield return modBotVersionRequest.SendWebRequest();

                if (modBotVersionRequest.result != UnityWebRequest.Result.Success || !isOnTitleScreen())
                    yield break;

                string newestModBotVersion = modBotVersionRequest.downloadHandler.text.Replace("\"", ""); // Latest ModBot version

                if (!isCloudVersionNewer(installedModBotVersion, newestModBotVersion))
                {
                    string modBotUpToDateMessage = ModBotLocalizationManager.FormatLocalizedStringFromID("modbotuptodate", installedModBotVersion);
                    debug.Log(modBotUpToDateMessage, Color.green);
                    yield break;
                }

                string message = ModBotLocalizationManager.FormatLocalizedStringFromID("newversion_message", newestModBotVersion, installedModBotVersion);
                string dismissButtonText = LocalizationManager.Instance.GetTranslatedString("newversion_dismiss");
                string installButtonText = LocalizationManager.Instance.GetTranslatedString("newversion_install");
                Generic2ButtonDialogue generic = new Generic2ButtonDialogue(message, dismissButtonText, null, installButtonText, onInstallButtonClicked);
                generic.SetColorOfFirstButton(Color.red);
                generic.SetColorOfSecondButton(Color.green);
            }
        }

        bool isOnTitleScreen()
        {
            return GameModeManager.IsOnTitleScreen() || (GameFlowManager.Instance && GameFlowManager.Instance.IsOnTitleScreen()) || (LevelManager.Instance && LevelManager.Instance.GetCurrentLevelID() == "VR_MainMenu");
        }

        bool isCloudVersionNewer(string installedVersion, string cloudVersion)
        {
            if (Version.TryParse(cloudVersion, out Version cloud))
            {
                if (Version.TryParse(installedVersion, out Version installed))
                {
                    return cloud > installed;
                }
                else
                {
                    throw new Exception("The installed version string was invalid");
                }
            }
            else
            {
                throw new Exception("The cloud version string was invalid");
            }
        }

        void onInstallButtonClicked()
        {
            Application.OpenURL("https://modbot.org/");
        }
    }
}
