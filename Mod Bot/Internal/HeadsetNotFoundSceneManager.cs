using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace InternalModBot
{
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
            if(scene.name == "NoVrHeadsetFoundScene")
            {
                StartupManager.OnStartUpNoHeadset();
                patchScene(scene);
            }
        }

        private void patchScene(Scene scene)
        {

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
