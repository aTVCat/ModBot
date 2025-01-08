using ModLibrary;
using Newtonsoft.Json;
using System;
using System.Collections;
using UnityEngine.Networking;

namespace InternalModBot
{
    internal static class ModBotWebsiteManager
    {
        public static void RequestAllModInfos(Action<UnityWebRequest> webRequestVariable, Action<ModsHolder?> downloadedData, Action<string> onCaughtError = null)
        {
            _ = StaticCoroutineRunner.StartStaticCoroutine(requestAllModInfos(delegate (UnityWebRequest r) { if(webRequestVariable != null) webRequestVariable(r); }, downloadedData, onCaughtError));
        }

        private static IEnumerator requestAllModInfos(Action<UnityWebRequest> webRequestVariable, Action<ModsHolder?> downloadedData, Action<string> onCaughtError = null)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get("https://modbot.org/api?operation=getAllModInfos"))
            {
                webRequest.timeout = 9;

                if(webRequestVariable != null)
                    webRequestVariable(webRequest);

                yield return StaticCoroutineRunner.StartStaticCoroutine(updateProgressOfAsyncOperation(webRequest.SendWebRequest()));

                if (webRequest.result != UnityWebRequest.Result.Success)
                {
                    if (onCaughtError != null)
                        onCaughtError("Cannot load mods page. Error details: " + webRequest.error + "\nTry visiting the website");

                    yield break;
                }

                ModsHolder? modsHolder = JsonTools.DeserializeCustomSettings<ModsHolder>(webRequest.downloadHandler.text, null);
                downloadedData(modsHolder);
            }
            yield break;
        }

        private static IEnumerator updateProgressOfAsyncOperation(UnityWebRequestAsyncOperation operation)
        {
            ModBotUIRoot.Instance.LoadingBar.SetProgress(0f);
            while (!operation.isDone)
            {
                ModBotUIRoot.Instance.LoadingBar.SetProgress(operation.progress);
                yield return null;
            }
            ModBotUIRoot.Instance.LoadingBar.SetProgress(1f);
        }
    }
}
