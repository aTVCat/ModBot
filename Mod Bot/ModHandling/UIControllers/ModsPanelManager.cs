using ModLibrary;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;
using VR.UI;

namespace InternalModBot
{
    /// <summary>
    /// Used by Mod-Bot to control most of the UI in Mod-Bot, this has control over the mod buttons and mods window. Note that all functions and fields on this class are private since they more or less work on their own.
    /// </summary>
    public class ModsPanelManager : Singleton<ModsPanelManager>
    {
        /// <summary>
        /// The color used for disabled mods
        /// </summary>
        public readonly Color DisabledModColor = new Color32(123, 14, 14, 255);

        private Action _actionOnModsPanelClose = null;

        private MainMenuUI _vrMainMenu;

        private VRPauseMenu _vrPauseMenu;

        private bool _isShowingModsMenuOverPauseMenu;

        private void Start()
        {
            // maybe delete this later since modbot will not port the game to pc
            if (DelegateScheduler.Instance)
            {
                DelegateScheduler.Instance.Schedule(delegate
                {
                    if (GameModeManager.IsOnTitleScreen() || GameModeManager.IsInLevelEditor())
                    {
                        DestroyOnSceneLoaded[] objs = FindObjectsOfType<DestroyOnSceneLoaded>();
                        foreach (var obj in objs)
                        {
                            if (obj.name.StartsWith("VRLoadingScene"))
                                Destroy(obj.gameObject);
                        }
                    }
                }, 1f);
            }

            _vrPauseMenu = FindObjectOfType<VRPauseMenu>();
            if (_vrPauseMenu)
                PatchVRPauseMenu(_vrPauseMenu);

            ModBotUIRoot.Instance.ModsWindow.WindowObject.SetActive(false);

            ModBotUIRoot.Instance.ModsWindow.CloseButton.onClick.AddListener(closeModsMenu); // Add close menu button callback
            ModBotUIRoot.Instance.ModsWindow.GetMoreModsButton.onClick.AddListener(onGetMoreModsClicked); // Add more mods clicked callback
            ModBotUIRoot.Instance.ModsWindow.OpenModsFolderButton.onClick.AddListener(onModsFolderClicked); // Add mods folder clicked callback

            ReloadModItems();

            ModBotUIRoot.Instance.gameObject.AddComponent<ModBotUIRootNew>().Init();
        }

        internal void PatchVRMainMenu(VR.UI.MainMenuUI mainMenuUI)
        {
            if (GameModeManager.IsInLevelEditor())
                return;

            _vrMainMenu = mainMenuUI;

            RectTransform settingsButton = TransformUtils.FindChildRecursive(mainMenuUI.transform, "SettingsButton") as RectTransform;
            RectTransform creditsButton = TransformUtils.FindChildRecursive(mainMenuUI.transform, "CreditsButton") as RectTransform;
            RectTransform topArea = settingsButton.parent as RectTransform;

            // adjust main menu height to fit mods button
            RectTransform mainMenuRoot = topArea.parent as RectTransform;
            Vector2 mainMenuRootSizeDelta = mainMenuRoot.sizeDelta;
            mainMenuRootSizeDelta.y = 210f;
            mainMenuRoot.sizeDelta = mainMenuRootSizeDelta;

            RectTransform modsButton = Instantiate(settingsButton, topArea);
            Button button = modsButton.GetComponent<Button>();
            button.onClick = new Button.ButtonClickedEvent();
            button.onClick.AddListener(delegate
            {
                if (_isShowingModsMenuOverPauseMenu && isModsMenuActive())
                {
                    closeModsMenu();
                }

                _isShowingModsMenuOverPauseMenu = false;
                ModBotUIRoot.Instance.SetTransform(mainMenuUI.transform.position + (mainMenuUI.transform.forward * -0.6f) + (mainMenuUI.transform.up * 0.2f), mainMenuUI.transform.eulerAngles, 0.004f);
            });
            button.onClick.AddListener(openModsMenu);

            TMPro.TextMeshProUGUI label = modsButton.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            label.text = "Mods";

            // lower other buttons
            settingsButton.anchoredPosition += Vector2.down * 35f;
            creditsButton.anchoredPosition += Vector2.down * 35f;
        }

        internal void PatchVRPauseMenu(VRPauseMenu pauseMenu)
        {
            RectTransform settingsButton = TransformUtils.FindChildRecursive(pauseMenu.transform, "SettingsButton") as RectTransform;
            RectTransform topArea = settingsButton.parent as RectTransform;

            // adjust pause menu height to fit mods button
            RectTransform mainMenuRoot = topArea.parent as RectTransform;
            Vector2 mainMenuRootSizeDelta = mainMenuRoot.sizeDelta;
            mainMenuRootSizeDelta.y = 210f;
            mainMenuRoot.sizeDelta = mainMenuRootSizeDelta;

            RectTransform modsButton = Instantiate(settingsButton, topArea);
            Button button = modsButton.GetComponent<Button>();
            button.onClick = new Button.ButtonClickedEvent();
            button.onClick.AddListener(delegate
            {
                if(!_isShowingModsMenuOverPauseMenu && isModsMenuActive())
                {
                    closeModsMenu();
                }

                _isShowingModsMenuOverPauseMenu = true;
                ModBotUIRoot.Instance.SetTransform(pauseMenu.transform.position + (pauseMenu.transform.forward * 1.6f) + (pauseMenu.transform.up * 2.1f), pauseMenu.transform.eulerAngles, 0.003f);
            });
            button.onClick.AddListener(openModsMenu);

            TMPro.TextMeshProUGUI label = modsButton.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            label.text = "Mods";
        }

        private bool isModsMenuActive()
        {
            return ModBotUIRoot.Instance.ModsWindow.WindowObject.activeSelf;
        }

        /// <summary>
        /// Opens mods panel. I actually made it for CDO mod
        /// </summary>
        /// <param name="onWindowClose"></param>
        public void OpenModsWindows(Action onWindowClose = null)
        {
            _actionOnModsPanelClose = onWindowClose;
            openModsMenu();
        }

        private void openModsMenu()
        {
            if (_isShowingModsMenuOverPauseMenu)
            {
                if (_vrPauseMenu && _vrPauseMenu.IsVisible())
                    _vrPauseMenu.MainMenu.SetActive(false);
            }
            else
            {
                if(_vrMainMenu)
                _vrMainMenu._root.gameObject.SetActive(false);
            }

            //GameUIRoot.Instance.SetEscMenuDisabled(true);

            ModBotUIRoot.Instance.ModsWindow.WindowObject.SetActive(true);
            ModBotUIRoot.Instance.ModsWindow.CreateModButton.gameObject.SetActive(ModCreationWindow.CanBeShown);
            ReloadModItems();
        }

        private void closeModsMenu()
        {
            if (_isShowingModsMenuOverPauseMenu)
            {
                if (_vrPauseMenu && _vrPauseMenu.IsVisible())
                    _vrPauseMenu.MainMenu.SetActive(true);
            }
            else
            {
                if (_vrMainMenu)
                    _vrMainMenu._root.gameObject.SetActive(true);
            }

            ModBotUIRoot.Instance.ModsWindow.WindowObject.SetActive(false);
            //GameUIRoot.Instance.SetEscMenuDisabled(false);

            if (_actionOnModsPanelClose != null)
            {
                _actionOnModsPanelClose();
            }

            _actionOnModsPanelClose = null;
        }

        private void onGetMoreModsClicked()
        {
            ModBotUIRootNew.DownloadWindow.Show();
        }

        private static void onModInfosLoadingEnd(ModsHolder? loadedData)
        {
            ModBotUIRoot.Instance.ModDownloadPage.LoadingPopup.gameObject.SetActive(false);
            if (loadedData == null)
            {
                return;
            }
            GameObject modDownloadInfoPrefab = InternalAssetBundleReferences.ModBot.GetObject("ModDownloadInfo");
            foreach (ModInfo modInfo in loadedData.Value.Mods)
            {
                ModBotUIRoot.Instance.ModDownloadPage.StartCoroutine(downloadSpecialModDataAndAdd(ModBotUIRoot.Instance.ModDownloadPage.Content.gameObject, modInfo, modDownloadInfoPrefab));
            }
        }

        private static void onModInfosLoadingError(string error)
        {
            ModBotUIRoot.Instance.ModDownloadPage.ErrorText.text = error;
            ModBotUIRoot.Instance.ModDownloadPage.ErrorWindow.SetActive(true);
        }

        private void onModsFolderClicked()
        {
            Process.Start(ModsManager.Instance.ModFolderPath);
        }

        private static IEnumerator downloadSpecialModDataAndAdd(GameObject content, ModInfo modInfo, GameObject modDownloadInfoPrefab)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get("https://modbot.org/api?operation=getSpecialModData&id=" + modInfo.UniqueID))
            {
                yield return webRequest.SendWebRequest(); // wait for the web request to send

                if (webRequest.isNetworkError || webRequest.isHttpError)
                    yield break;

                Dictionary<string, JToken> specialModData = JsonConvert.DeserializeObject<Dictionary<string, JToken>>(webRequest.downloadHandler.text);

                if (!specialModData["Verified"].ToObject<bool>()) // do not want unchecked mods to come up in-game.
                    yield break;
            }

            GameObject holder = Instantiate(modDownloadInfoPrefab);
            holder.transform.SetParent(content.transform, false);
            holder.AddComponent<ModDownloadInfoItem>().Init(modInfo);
        }

        private void openModsOptionsWindowForMod(LoadedModInfo mod)
        {
            ModOptionsWindowBuilder builder = new ModOptionsWindowBuilder(ModBotUIRoot.Instance.ModsWindow.WindowObject, mod.ModReference);
            mod.ModReference.CreateSettingsWindow(builder);
        }

        private void toggleIsModDisabled(LoadedModInfo mod)
        {
            if (mod == null)
                return;
            mod.IsEnabled = !mod.IsEnabled;

            ReloadModItems();
        }

        private void addModToList(LoadedModInfo mod, GameObject parent)
        {
            bool isModActive = mod.IsEnabled;

            GameObject modItem = InternalAssetBundleReferences.ModBot.InstantiateObject("ModItemPrefab");
            modItem.transform.SetParent(parent.transform, false);

            string modName = mod.OwnerModInfo.DisplayName;

            _modItems.Add(modItem);

            ModdedObject modItemModdedObject = modItem.GetComponent<ModdedObject>();

            modItemModdedObject.GetObject<Text>(0).text = modName; // Set title
            modItemModdedObject.GetObject<Text>(1).text = mod.OwnerModInfo.Description; // Set description
            modItemModdedObject.GetObject<Text>(5).text = ModBotLocalizationManager.FormatLocalizedStringFromID("mods_menu_mod_id", mod.OwnerModInfo.UniqueID);

            modItemModdedObject.GetObject<RawImage>(2).texture = mod.OwnerModInfo.HasImage ? mod.OwnerModInfo.CachedImage : null;

            Button enableOrDisableButton = modItem.GetComponent<ModdedObject>().GetObject<Button>(3);

            if (!isModActive)
            {
                modItem.GetComponent<Image>().color = DisabledModColor;
                LocalizedTextField localizedTextField = enableOrDisableButton.transform.GetChild(0).GetComponent<LocalizedTextField>();
                localizedTextField.LocalizationID = "mods_menu_enable_mod";
                localizedTextField.tryLocalizeTextField();
                localizedTextField.gameObject.SetActive(true);

                enableOrDisableButton.colors = new ColorBlock() { normalColor = Color.green * 1.2f, highlightedColor = Color.green, pressedColor = Color.green * 0.8f, colorMultiplier = 1 };
            }
            enableOrDisableButton.interactable = true;

            Button BroadcastButton = modItemModdedObject.GetObject<Button>(6);
            BroadcastButton.onClick.AddListener(delegate { onBroadcastButtonClicked(mod.ModReference); });
            BroadcastButton.gameObject.SetActive(false);

            Button deleteModButton = modItemModdedObject.GetObject_Alt<Button>(7);
            deleteModButton.gameObject.SetActive(false);

            modItemModdedObject.GetObject_Alt<Transform>(8).gameObject.SetActive(true);

            modItemModdedObject.GetObject<Button>(3).onClick.AddListener(delegate { toggleIsModDisabled(mod); }); // Add disable button callback
            //modItemModdedObject.GetObject<Button>(4).GetComponentInChildren<Text>().gameObject.AddComponent<LocalizedTextField>().LocalizationID = "mods_menu_mod_options";
            Button modsOptionButton = modItemModdedObject.GetObject<Button>(4);
            modsOptionButton.onClick.AddListener(delegate { openModsOptionsWindowForMod(mod); }); // Add Mod Options button callback
            modsOptionButton.interactable = mod.ModReference != null && mod.ModReference.ImplementsSettingsWindow() && isModActive;

        }

        private static void onBroadcastButtonClicked(Mod mod)
        {
        }

        private void Update()
        {
            /*if (!ModBotUIRootNew.DownloadWindow.gameObject.activeInHierarchy)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    closeModsMenu();
                }
            }*/
            //if (ModsDownloadManager.IsLoadingModInfos())
            //{
            //    UnityWebRequest r = ModsDownloadManager.GetModInfosWebRequest();
            //    ModBotUIRoot.Instance.ModDownloadPage.ProgressBarSlider.value = r.downloadProgress;
            //}
        }


        /// <summary>
        /// Refereshes what mods should be displayed in the mods menu
        /// </summary>
        public void ReloadModItems()
        {
            _modItems.Clear();

            // Remove all mods from list
            foreach (Transform child in ModBotUIRoot.Instance.ModsWindow.Content.transform)
            {
                Destroy(child.gameObject);
            }

            List<LoadedModInfo> mods = ModsManager.Instance.GetAllMods();

            // Set the Content panel (ModdedObjectModsWindow.objects[0]) to appropriate height
            RectTransform size = ModBotUIRoot.Instance.ModsWindow.Content.GetComponent<RectTransform>();
            size.sizeDelta = new Vector2(size.sizeDelta.x, MOD_ITEM_HEIGHT * mods.Count);

            // Add all mods back to list
            foreach (LoadedModInfo info in mods)
            {
                addModToList(info, ModBotUIRoot.Instance.ModsWindow.Content);
            }
        }

        private readonly List<GameObject> _modItems = new List<GameObject>();
        private const int MOD_ITEM_HEIGHT = 100;
    }
}
