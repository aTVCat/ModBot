using InternalModBot.LevelEditor;
using ModLibrary;
using System;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace InternalModBot
{
    /// <summary>
    /// Used to start Mod-Bot when the game starts
    /// </summary>
    public static class StartupManager
    {
        /// <summary>
        /// Sets up Mod-Mot in general, called on game start
        /// </summary>
        public static void OnStartUp()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            // this is temp
            string str = SceneTransitionManager.Instance.VRLoadingScenePrefab.name;
            foreach(GameObject gameObject in SceneManager.GetActiveScene().GetRootGameObjects())
            {
                if (gameObject.name.Contains(str))
                    gameObject.SetActive(false);
            }

            ModBotHarmonyInjectionManager.TryInject();

            if (!Directory.Exists(AssetLoader.GetModsFolderDirectory())) // If the mods folder does not exist, something probably went wrong during installation
                throw new DirectoryNotFoundException("Mods folder not found!");

            GameObject modBotManagers = new GameObject("ModBotManagers");

            modBotManagers.AddComponent<ModsManager>();
            modBotManagers.AddComponent<UpdateChecker>();                     // Checks for new Mod-Bot versions
            modBotManagers.AddComponent<ModsPanelManager>();                  // Adds the mods button in the main menu and pause screen
            modBotManagers.AddComponent<DebugLineDrawingManager>();           // Handles drawing lines on screen
            modBotManagers.AddComponent<VersionLabelManager>();               // Handles custom version label stuff

            try // If an exception is thrown here, the crash screen wont appear, so we have to implement our own
            {
                initilizeUI(); // Initialize all custom UI

                ModsManager.Instance.Initialize(); // Loads all mods in the mods folder
            }
            catch (Exception e)
            {
                debug.Log(e.Message + "\n" + e.StackTrace, Color.red);

                ModBotUIRoot.Instance.ConsoleUI.Animator.Play("hideConsole");
            }

            GlobalEventManager.Instance.AddEventListener(GlobalEvents.LevelEditorStarted, new Action(ModsManager.Instance.PassOnMod.OnLevelEditorStarted));

            IgnoreCrashesManager.Start();

            stopwatch.Stop();
            debug.Log("Initialized Mod-Bot in " + stopwatch.Elapsed.TotalSeconds + " seconds");
        }

        static void initilizeUI()
        {
            GameObject spawnedUI = InternalAssetBundleReferences.ModBot.InstantiateObject("Canvas");

            ModdedObject spawedUIModdedObject = spawnedUI.GetComponent<ModdedObject>();
            ModBotUIRoot modBotUIRoot = spawnedUI.AddComponent<ModBotUIRoot>();
            modBotUIRoot.Init(spawedUIModdedObject);
        }
    }
}
