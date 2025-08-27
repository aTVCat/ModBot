using ModLibrary;
using UnityEngine;

namespace InternalModBot
{
    /// <summary>
    /// The UI root for all mod-bot UI
    /// </summary>
    internal class ModBotUIRoot : Singleton<ModBotUIRoot>
    {
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
        /// Mods download window
        /// </summary>
        public ModDownloadWindow DownloadWindow;
        /// <summary>
        /// Loading bar
        /// </summary>
        public GenericLoadingBar LoadingBar;
        /// <summary>
        /// UIController UI inside debug laptop
        /// </summary>
        private ConsoleUI _consoleUi;

        /// <summary>
        /// Sets up the mod-bot UI from a modded object
        /// </summary>
        /// <param name="moddedObject"></param>
        public void Init(ModdedObject moddedObject)
        {
            Root = moddedObject.GetComponent<Canvas>();

            ModBotSignInUI = gameObject.AddComponent<ModBotSignInUI>();
            ModBotSignInUI.Init(moddedObject.GetObject<ModdedObject>(6));

            ModsWindow = gameObject.AddComponent<ModsWindow>();
            ModsWindow.Init(moddedObject.GetObject<ModdedObject>(7));

            Generic2ButtonDialogeUI = gameObject.AddComponent<Generic2ButtonDialogeUI>();
            Generic2ButtonDialogeUI.Init(moddedObject.GetObject<ModdedObject>(8));

            ModOptionsWindow = gameObject.AddComponent<ModOptionsWindow>();
            ModOptionsWindow.Init(moddedObject.GetObject<ModdedObject>(9));
            ModOptionsWindow.WindowObject.SetActive(false);

            DownloadWindow = moddedObject.GetObject<GameObject>(12).AddComponent<ModDownloadWindow>().Init();

            LoadingBar = moddedObject.GetObject<GameObject>(13).AddComponent<GenericLoadingBar>().Init();
        }

        public void SetTransform(Vector3 worldPosition, Vector3 eulerAngles, float scale)
        {
            base.transform.position = worldPosition;
            base.transform.eulerAngles = eulerAngles;
            base.transform.localScale = Vector3.one * scale;
        }

        public void SetConsoleUI(ConsoleUI consoleUi)
        {
            _consoleUi = consoleUi;
        }

        public ConsoleUI GetConsoleUI()
        {
            return _consoleUi;
        }
    }
}
