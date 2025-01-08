using UnityEngine;
using UnityEngine.SceneManagement;

namespace InternalModBot
{
    /// <summary>
    /// A manager that initializes mod bot when NoVrHeadsetFoundScene scene loads in
    /// </summary>
    public class HeadsetNotFoundSceneManager : Singleton<HeadsetNotFoundSceneManager>
    {
        internal void Initialize()
        {
            SceneManager.sceneLoaded += onSceneLoaded;
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= onSceneLoaded;
        }

        private void onSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            if (scene.name == "NoVrHeadsetFoundScene")
                StartupManager.OnStartUpNoHeadset();
        }

        public static void Create()
        {
            if (Instance)
                return;

            GameObject gameObject = new GameObject("ModBot HeadsetNotFoundScene Manager");
            gameObject.AddComponent<HeadsetNotFoundSceneManager>().Initialize();
            DontDestroyOnLoad(gameObject);
        }
    }
}
