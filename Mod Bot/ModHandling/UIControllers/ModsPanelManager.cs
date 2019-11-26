﻿using ModLibrary;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace InternalModBot
{
    /// <summary>
    /// Used by Mod-Bot to control most of the UI in Mod-Bot, this has control over the mod buttons and mods window. Note that all functions and fields on this class are private since they more or less work on their own.
    /// </summary>
    public class ModsPanelManager : Singleton<ModsPanelManager>
    {
        void Start()
        {
            Vector3 pauseScreenButtonOffset = new Vector3(0f, 1.2f, 0f);

            GameObject titleScreenContainer = GameUIRoot.Instance.TitleScreenUI.RootButtonsContainer.GetChild(0).GetChild(7).gameObject; // Gets the lower buttons container

            // Copy the options button to make into the Mods button
            GameObject modsButtonPrefab = titleScreenContainer.transform.GetChild(1).gameObject; // Gets the options button (we copy it and replace its organs and face)
            GameObject mainMenuModsButton = Instantiate(modsButtonPrefab, titleScreenContainer.transform);

            mainMenuModsButton.GetComponentInChildren<Text>().text = "MODS"; // Set title
            mainMenuModsButton.transform.SetAsFirstSibling();

            GameObject pauseScreenModsButton = Instantiate(GameUIRoot.Instance.EscMenu.SettingsButton.transform.gameObject, GameUIRoot.Instance.EscMenu.SettingsButton.transform.parent); // All of these lines edit the buttons on the pause menu
            GameUIRoot.Instance.EscMenu.ReturnToGameButton.transform.position += pauseScreenButtonOffset;
            GameUIRoot.Instance.EscMenu.SettingsButton.transform.position += pauseScreenButtonOffset;
            GameUIRoot.Instance.EscMenu.transform.GetChild(1).GetChild(9).position -= pauseScreenButtonOffset;
            GameUIRoot.Instance.EscMenu.ExitButton.transform.position -= pauseScreenButtonOffset;
            GameUIRoot.Instance.EscMenu.ExitConfirmUI.transform.position -= pauseScreenButtonOffset;
            GameUIRoot.Instance.EscMenu.MainMenuButton.transform.position -= pauseScreenButtonOffset;
            GameUIRoot.Instance.EscMenu.MainMenuConfirmUI.transform.position -= pauseScreenButtonOffset;

            pauseScreenModsButton.transform.position -= pauseScreenButtonOffset;
            pauseScreenModsButton.GetComponentInChildren<Text>().text = "MODS";

            GameObject modsWindowPrefab = AssetLoader.GetObjectFromFile("modswindow", "ModsMenu", "Clone Drone in the Danger Zone_Data/");
            _modsWindow = Instantiate(modsWindowPrefab);

            _modsWindowModdedObject = _modsWindow.GetComponent<ModdedObject>();
            _modsWindow.SetActive(false);

            mainMenuModsButton.GetComponent<Button>().onClick = new Button.ButtonClickedEvent(); // This is used to remove the persistent listeners that the options button has
            mainMenuModsButton.GetComponent<Button>().onClick.AddListener(openModsMenu); // Add open menu callback
            pauseScreenModsButton.GetComponent<Button>().onClick = new Button.ButtonClickedEvent(); // This is used to remove the persistent listeners that the options button has
            pauseScreenModsButton.GetComponent<Button>().onClick.AddListener(openModsMenu); // Add open menu callback

            _modsWindowModdedObject.GetObject<Button>(1).onClick.AddListener(closeModsMenu); // Add close menu button callback
            _modsWindowModdedObject.GetObject<Button>(2).onClick.AddListener(onGetMoreModsClicked); // Add more mods clicked callback

            Transform image = Instantiate(GameUIRoot.Instance.TitleScreenUI.CreditsUI.transform.GetChild(1), GameUIRoot.Instance.TitleScreenUI.CreditsUI.transform);
            image.gameObject.SetActive(true);
            image.GetComponent<Image>().sprite = AssetLoader.GetObjectFromFile<Sprite>("modswindow", "modbot", "Clone Drone in the Danger Zone_Data/");
            image.GetComponent<RectTransform>().localScale = new Vector3(image.GetComponent<RectTransform>().localScale.x * 1.5f, image.GetComponent<RectTransform>().localScale.y * 0.375f, 1f);
            image.GetComponent<RectTransform>().position -= new Vector3(7f, 0f);

            Transform spawnedObject = Instantiate(GameUIRoot.Instance.TitleScreenUI.CreditsUI.transform.GetChild(4), GameUIRoot.Instance.TitleScreenUI.CreditsUI.transform);
            spawnedObject.gameObject.SetActive(true);
            spawnedObject.GetComponent<Text>().text = "X606, Gorakh,\n<size=9>Niek_Alexander and HSCarsten</size>";
            spawnedObject.GetComponent<RectTransform>().position -= new Vector3(7f, -2f);

            Transform upperTitle = Instantiate(GameUIRoot.Instance.TitleScreenUI.CreditsUI.transform.GetChild(3), GameUIRoot.Instance.TitleScreenUI.CreditsUI.transform);
            upperTitle.gameObject.SetActive(true);
            upperTitle.GetComponent<Text>().text = "Mod-Bot by:";
            upperTitle.GetComponent<Text>().color = new Color32(255, 165, 0, 255);
            upperTitle.GetComponent<RectTransform>().position -= new Vector3(7f, -2f);

            GameUIRoot.Instance.TitleScreenUI.CreditsUI.transform.GetChild(1).GetComponent<RectTransform>().position += new Vector3(7f, 0f);
            GameUIRoot.Instance.TitleScreenUI.CreditsUI.transform.GetChild(3).GetComponent<RectTransform>().position += new Vector3(7f, 0f);
            GameUIRoot.Instance.TitleScreenUI.CreditsUI.transform.GetChild(4).GetComponent<RectTransform>().position += new Vector3(7f, 0f);

            ReloadModItems();
        }

        void openModsMenu()
        {
            _modsWindow.SetActive(true);
            ReloadModItems();
        }

        void closeModsMenu()
        {
            _modsWindow.SetActive(false);
        }

        void onGetMoreModsClicked()
        {
            GameObject modDownloadsPrefab = AssetLoader.GetObjectFromFile<GameObject>("modswindow", "Mod downloads", "Clone Drone in the Danger Zone_Data/");
            
            ModdedObject spawnedModdedObject = Instantiate(modDownloadsPrefab).GetComponent<ModdedObject>();
            GameObject content = spawnedModdedObject.GetObject<GameObject>(0);
            spawnedModdedObject.GetObject<Button>(1).onClick.AddListener(delegate
            {
                Destroy(spawnedModdedObject.gameObject);
            });

            spawnedModdedObject.StartCoroutine(downloadModData(content));
        }

        static IEnumerator downloadModData(GameObject content)
        {
            GameObject modDownloadInfoPrefab = AssetLoader.GetObjectFromFile<GameObject>("modswindow", "ModDownloadInfo", "Clone Drone in the Danger Zone_Data/");
            
            UnityWebRequest webRequest = UnityWebRequest.Get("https://modbot-d8a58.firebaseio.com/mods/.json");

            yield return webRequest.SendWebRequest(); // wait for the web request to send

            if(webRequest.isNetworkError || webRequest.isHttpError)
                yield break;

            TransformUtils.DestroyAllChildren(content.transform);

            ModsHolder mods = Newtonsoft.Json.JsonConvert.DeserializeObject<ModsHolder>(webRequest.downloadHandler.text);

            foreach(ModsHolder.ModHolder mod in mods.Mods)
            {
                if(!mod.Checked) // do not want unchecked mods to come up in-game.
                    continue;

                GameObject holder = Instantiate(modDownloadInfoPrefab);
                holder.transform.parent = content.transform;
                holder.AddComponent<ModDownloadInfoItem>().Init(mod);
            }

        }

        void openModsOptionsWindowForMod(Mod mod)
        {
            ModOptionsWindowBuilder builder = new ModOptionsWindowBuilder(_modsWindow, mod);
            mod.CreateSettingsWindow(builder);
        }

        void toggleIsModDisabled(int ID)
        {
            Mod mod = ModsManager.Instance.GetAllMods()[ID];
            bool? isNotActive = ModsManager.Instance.IsModDeactivated(mod);

            if (!isNotActive.HasValue)
                return;
            
            if (isNotActive.Value)
            {
                ModsManager.Instance.EnableMod(mod);
            }
            else
            {
                ModsManager.Instance.DisableMod(mod);
            }
            
            ReloadModItems();
        }

        void addModToList(Mod mod, GameObject parent)
        {
            bool? isModNotActive = ModsManager.Instance.IsModDeactivated(mod);
            if (!isModNotActive.HasValue)
                return;

            GameObject modItemPrefab = AssetLoader.GetObjectFromFile("modswindow", "ModItemPrefab", "Clone Drone in the Danger Zone_Data/");
            GameObject modItem = Instantiate(modItemPrefab, parent.transform);

            string modName = mod.GetModName();
            string url = mod.GetModImageURL();

            _modItems.Add(modItem);

            if (!string.IsNullOrEmpty(url))
                setImageFromURL(url, mod);

            ModdedObject modItemModdedObject = modItem.GetComponent<ModdedObject>();

            modItemModdedObject.GetObject<Text>(0).text = modName; // Set title
            modItemModdedObject.GetObject<Text>(1).text = mod.GetModDescription(); // Set description
            modItemModdedObject.GetObject<Text>(5).text = "Mod ID: " + mod.GetUniqueID();

            if (isModNotActive.Value)
            {
                modItem.GetComponent<Image>().color = Color.red;
                Button disableButton = modItem.GetComponent<ModdedObject>().GetObject<Button>(3);
                disableButton.transform.GetChild(0).GetComponent<Text>().text = "Enable Mod";
                disableButton.colors = new ColorBlock() { normalColor = Color.green * 1.2f, highlightedColor = Color.green, pressedColor = Color.green * 0.8f, colorMultiplier = 1 };
            }

            Button BroadcastButton = modItemModdedObject.GetObject<Button>(6);
            BroadcastButton.onClick.AddListener( delegate { onBroadcastButtonClicked(mod); } );
            BroadcastButton.gameObject.SetActive(GameModeManager.IsMultiplayer());

            Button DownloadButton = modItemModdedObject.GetObject<Button>(7);
            DownloadButton.onClick.AddListener(delegate { onDownloadButtonClicked(mod); });
            bool hasNoFile = ModsManager.Instance.GetIsModOnlyLoadedInMemory(mod);
            DownloadButton.gameObject.SetActive(hasNoFile);

            int modId = ModsManager.Instance.GetAllMods().IndexOf(mod);
            modItemModdedObject.GetObject<Button>(3).onClick.AddListener(delegate { toggleIsModDisabled(modId); }); // Add disable button callback
            modItemModdedObject.GetObject<Button>(4).onClick.AddListener(delegate { openModsOptionsWindowForMod(mod); }); // Add Mod Options button callback
            modItemModdedObject.GetObject<Button>(4).interactable = mod.ImplementsSettingsWindow();
        }

        static void onBroadcastButtonClicked(Mod mod)
        {
            Generic2ButtonDialogue dialoge = new Generic2ButtonDialogue("This will ask everyone else in the server using Mod-Bot to download " + mod.GetModName() + ", are you sure you want to do this?",
                "No", null,
                "Yes", delegate
                {
                    ModSharingManager.SendModToAllModBotClients(ModsManager.Instance.GetModData(mod), mod.GetModName());
                });
        }

        void onDownloadButtonClicked(Mod mod)
        {
            Generic2ButtonDialogue dialoge = new Generic2ButtonDialogue("This will place " + mod.GetModName() + " in your mod folder as a .dll file, are you sure you want to do this?", 
                "No", null, 
                "Yes", delegate 
                {
                    ModsManager.Instance.WriteDllFileToModFolder(mod);
                    ReloadModItems();
                });
        }

        void setImageFromURL(string url, Mod owner)
        {
            if (string.IsNullOrEmpty(url))
                return;

            StartCoroutine(setModImageFromURLRoutine(owner, url));
        }

        IEnumerator setModImageFromURLRoutine(Mod mod, string url)
        {
            UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(url);

            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError || webRequest.isHttpError)
            {
                debug.Log("Error dowloading preview image for mod: \"" + mod.GetUniqueID() + "\":\n" + webRequest.error, Color.red);
            }
            else
            {
                ModdedObject modWindowItem = findModItemWithName(mod.GetUniqueID());
                if (modWindowItem != null)
                {
                    DownloadHandlerTexture textureDownloader = webRequest.downloadHandler as DownloadHandlerTexture;
                    modWindowItem.GetObject<RawImage>(2).texture = textureDownloader.texture;
                }
                else
                {
                    debug.Log("Error: Could not find mod item in mods window for mod: \"" + mod.GetUniqueID() + "\"", Color.red);
                }
            }
        }

        void Update()
        {
            if (_modsWindow.activeInHierarchy)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    closeModsMenu();
                }
            }
        }
        
        ModdedObject findModItemWithName(string id)
        {
            foreach(GameObject moddedObject in _modItems)
            {
                if (moddedObject.GetComponent<ModdedObject>().GetObject<Text>(5).text == "Mod ID: " + id)
                {
                    return moddedObject.GetComponent<ModdedObject>();
                }
            }
            return null;
        }

        /// <summary>
        /// Refereshes what mods should be displayed in the mods menu
        /// </summary>
        public void ReloadModItems()
        {
            _modItems.Clear();

            // Remove all mods from list
            foreach (Transform child in ((GameObject)_modsWindowModdedObject.objects[0]).transform)
            {
                Destroy(child.gameObject);
            }

            List<Mod> mods = ModsManager.Instance.GetAllMods();

            // Set the Content panel (ModdedObjectModsWindow.objects[0]) to appropriate height
            RectTransform size = _modsWindowModdedObject.GetObject<GameObject>(0).GetComponent<RectTransform>();
            size.sizeDelta = new Vector2(size.sizeDelta.x, MOD_ITEM_HEIGHT * mods.Count);

            // Add all mods back to list
            for (int i = 0; i < mods.Count; i++)
            {
                addModToList(mods[i], _modsWindowModdedObject.GetObject<GameObject>(0));
            }
        }

        List<GameObject> _modItems = new List<GameObject>();

        GameObject _modsWindow;

        ModdedObject _modsWindowModdedObject;

        const int MOD_ITEM_HEIGHT = 100;
    }
}
