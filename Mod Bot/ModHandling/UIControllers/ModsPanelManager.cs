﻿using ModLibrary;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
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
        public readonly Color DisabledModColor = new Color(0.21f, 0.13f, 0f, 1f);

        private Action _actionOnModsPanelClose = null;

        private MainMenuUI _vrMainMenu;

        private VRPauseMenu _vrPauseMenu;

        private bool _isShowingModsMenuOverPauseMenu;

        private readonly List<GameObject> _modItems = new List<GameObject>();

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
            RectTransform bottomArea = TransformUtils.FindChildRecursive(mainMenuUI.transform, "BottomButtonArea") as RectTransform;
            bottomArea.anchoredPosition += Vector2.up * 10f; // raise exit button to not overlap version text

            // adjust main menu height to fit mods button
            RectTransform mainMenuRoot = topArea.parent as RectTransform;
            Vector2 mainMenuRootSizeDelta = mainMenuRoot.sizeDelta;
            mainMenuRootSizeDelta.y = 220f;
            mainMenuRoot.sizeDelta = mainMenuRootSizeDelta;

            RectTransform modsButton = Instantiate(settingsButton, topArea);
            modsButton.name = "ModsButton";
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
            modsButton.name = "ModsButton";
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

        private void onModsFolderClicked()
        {
            Process.Start(ModsManager.Instance.ModFolderPath);
        }

        private void openModsOptionsWindowForMod(LoadedModInfo mod)
        {
            ModOptionsWindowBuilder builder = new ModOptionsWindowBuilder(ModBotUIRoot.Instance.ModsWindow.WindowObject, mod.ModReference);
            mod.ModReference.CreateSettingsWindow(builder);
        }

        private void enableMod(LoadedModInfo mod)
        {
            if (mod == null)
                return;

            mod.IsEnabled = true;
            ReloadModItems();
        }

        private void disableMod(LoadedModInfo mod)
        {
            if (mod == null)
                return;

            mod.IsEnabled = false;
            ReloadModItems();
        }

        private void addModToList(LoadedModInfo mod, GameObject parent)
        {
            bool isModActive = mod.IsEnabled;

            GameObject modItem = InternalAssetBundleReferences.ModBot.InstantiateObject("ModItemPrefab");
            modItem.transform.SetParent(parent.transform, false);

            if (!isModActive)
                modItem.GetComponent<Image>().color = DisabledModColor;

            _modItems.Add(modItem);

            ModdedObject modItemModdedObject = modItem.GetComponent<ModdedObject>();

            modItemModdedObject.GetObject<TextMeshProUGUI>(0).text = mod.OwnerModInfo.DisplayName; // Set title
            modItemModdedObject.GetObject<TextMeshProUGUI>(1).text = mod.OwnerModInfo.Description; // Set description

            modItemModdedObject.GetObject<RawImage>(2).texture = mod.OwnerModInfo.HasImage ? mod.OwnerModInfo.CachedImage : null;

            Button disableButton = modItemModdedObject.GetObject<Button>(3);
            disableButton.onClick.AddListener(delegate { disableMod(mod); }); // Add disable button callback
            disableButton.gameObject.SetActive(isModActive);

            Button enableButton = modItemModdedObject.GetObject<Button>(5);
            enableButton.onClick.AddListener(delegate { enableMod(mod); }); // Add disable button callback
            enableButton.gameObject.SetActive(!isModActive);

            Button modsOptionButton = modItemModdedObject.GetObject<Button>(4);
            modsOptionButton.onClick.AddListener(delegate { openModsOptionsWindowForMod(mod); }); // Add Mod Options button callback
            modsOptionButton.interactable = mod.ModReference != null && mod.ModReference.ImplementsSettingsWindow() && isModActive;
        }

        /// <summary>
        /// Refreshes what mods should be displayed in the mods menu
        /// </summary>
        public void ReloadModItems()
        {
            _modItems.Clear();

            // Remove all mods from list
            TransformUtils.DestroyAllChildren(ModBotUIRoot.Instance.ModsWindow.Content.transform);

            // Add all mods back to list
            List<LoadedModInfo> mods = ModsManager.Instance.GetAllMods();
            foreach (LoadedModInfo info in mods)
            {
                addModToList(info, ModBotUIRoot.Instance.ModsWindow.Content);
            }
        }
    }
}
