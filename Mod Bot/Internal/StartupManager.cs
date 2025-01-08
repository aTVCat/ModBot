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
        private static bool _isInNoHeadsetMode;

        /// <summary>
        /// Sets up Mod-Mot in general, called on game start
        /// </summary>
        public static void OnStartUp()
        {
            _isInNoHeadsetMode = false;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

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
                initializeUI(); // Initialize all custom UI

                ModsManager.Instance.Initialize(); // Loads all mods in the mods folder
            }
            catch (Exception e)
            {
                debug.Log(e.Message + "\n" + e.StackTrace, Color.red);

                ModBotUIRoot.Instance.ConsoleUI.Animator.Play("hideConsole");
            }

            GlobalEventManager.Instance.AddEventListener(GlobalEvents.LevelEditorStarted, new Action(ModsManager.Instance.PassOnMod.OnLevelEditorStarted));

            IgnoreCrashesManager.Start();

            if (!GameModeManager.IsDevModeEnabled() && !GameVersionManager.IsRunningOnVRHeadset())
            {
                HeadsetNotFoundSceneManager.Create();
                return;
            }

            stopwatch.Stop();
            debug.Log("Initialized Mod-Bot in " + stopwatch.Elapsed.TotalSeconds + " seconds");
        }

        internal static void OnStartUpNoHeadset()
        {
            _isInNoHeadsetMode = true;

            ModBotHarmonyInjectionManager.TryInject();

            if (!Directory.Exists(AssetLoader.GetModsFolderDirectory()))
                throw new DirectoryNotFoundException("Mods folder not found!");

            GameObject modBotManagers = new GameObject("ModBotManagers");
            modBotManagers.AddComponent<ModsPanelManager>();
            ModsManager modsManager = modBotManagers.AddComponent<ModsManager>();

            initializeUI();
            modsManager.Initialize();
        }

        public static bool HasStartedWithNoHeadset()
        {
            return _isInNoHeadsetMode;
        }

        static void initializeUI()
        {
            GameObject spawnedUI = InternalAssetBundleReferences.ModBot.InstantiateObject("Canvas");
            if (_isInNoHeadsetMode)
            {
                Canvas canvas = spawnedUI.GetComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                canvas.sortingOrder = 2;
            }

            ModdedObject spawedUIModdedObject = spawnedUI.GetComponent<ModdedObject>();
            ModBotUIRoot modBotUIRoot = spawnedUI.AddComponent<ModBotUIRoot>();
            modBotUIRoot.Init(spawedUIModdedObject);
        }
    }
}
