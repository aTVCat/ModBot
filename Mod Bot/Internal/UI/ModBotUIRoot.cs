﻿using ModLibrary;
using UnityEngine;
using UnityEngine.UI;

namespace InternalModBot
{
    /// <summary>
    /// The UI root for all mod-bot UI
    /// </summary>
    internal class ModBotUIRoot : Singleton<ModBotUIRoot>
    {
        /// <summary>
        /// The Console UI
        /// </summary>
        public ConsoleUI ConsoleUI;
        /// <summary>
        /// The FPS counter in the corner
        /// </summary>
        public FPSCounterUI FPSCounter;
        /// <summary>
        /// The modbot sign in UI
        /// </summary>
        public ModBotSignInUI ModBotSignInUI;
        /// <summary>
        /// The mods window UI
        /// </summary>
        public ModsWindow ModsWindow;
        /// <summary>
        /// The generic 2 Button dialoge UI
        /// </summary>
        public Generic2ButtonDialogeUI Generic2ButtonDialogeUI;
        /// <summary>
        /// The mod options window UI
        /// </summary>
        public ModOptionsWindow ModOptionsWindow;
        /// <summary>
        /// The root canvas
        /// </summary>
        public Canvas Root;

        /// <summary>
        /// Sets up the mod-bot UI from a modded object
        /// </summary>
        /// <param name="moddedObject"></param>
        public void Init(ModdedObject moddedObject)
        {
            Root = moddedObject.GetComponent<Canvas>();

            ConsoleUI = gameObject.AddComponent<ConsoleUI>();
            ConsoleUI.Init(moddedObject.GetObject<Animator>(0), moddedObject.GetObject<GameObject>(1), moddedObject.GetObject<GameObject>(2), moddedObject.GetObject<InputField>(3));

            FPSCounter = gameObject.AddComponent<FPSCounterUI>();
            FPSCounter.Init(moddedObject.GetObject<Text>(4));

            ModBotSignInUI = gameObject.AddComponent<ModBotSignInUI>();
            ModBotSignInUI.Init(moddedObject.GetObject<ModdedObject>(6));

            ModsWindow = gameObject.AddComponent<ModsWindow>();
            ModsWindow.Init(moddedObject.GetObject<ModdedObject>(7));

            Generic2ButtonDialogeUI = gameObject.AddComponent<Generic2ButtonDialogeUI>();
            Generic2ButtonDialogeUI.Init(moddedObject.GetObject<ModdedObject>(8));

            ModOptionsWindow = gameObject.AddComponent<ModOptionsWindow>();
            ModOptionsWindow.Init(moddedObject.GetObject<ModdedObject>(9));
        }

        public void SetTransform(Vector3 worldPosition, Vector3 eulerAngles, float scale)
        {
            base.transform.position = worldPosition;
            base.transform.eulerAngles = eulerAngles;
            base.transform.localScale = Vector3.one * scale;
        }
    }

}
