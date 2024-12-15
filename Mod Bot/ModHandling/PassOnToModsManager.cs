// New mod loading system
using ModLibrary;
using System.Collections.Generic;
#pragma warning disable CS0618 // We dont care if its depricated sincei

namespace InternalModBot
{
    /// <summary>
    /// Used by Mod-Bot to call events on all loaded active mods, you probably dont want to use this from mods
    /// </summary>
    public class PassOnToModsManager : Mod
    {
        /// <summary>
        /// Calls this method on all mods
        /// </summary>
        protected internal override void OnModRefreshed()
        {
            List<Mod> mods = ModsManager.Instance.GetAllLoadedActiveMods();
            for (int i = 0; i < mods.Count; i++)
            {
                mods[i].OnModRefreshed();
            }
        }

        /// <summary>
        /// Calls this method on all mods
        /// </summary>
        protected internal override void OnLevelEditorStarted()
        {
#if MODDED_LEVEL_OBJECTS
            LevelEditorObjectAdder.OnLevelEditorStarted();
#endif
            List<Mod> mods = ModsManager.Instance.GetAllLoadedActiveMods();
            for (int i = 0; i < mods.Count; i++)
            {
                mods[i].OnLevelEditorStarted();
            }
        }

        /// <summary>
        /// Calls this method on all mods
        /// </summary>
        /// <param name="command"></param>
        protected internal override void OnCommandRan(string command)
        {
            List<Mod> mods = ModsManager.Instance.GetAllLoadedActiveMods();
            for (int i = 0; i < mods.Count; i++)
            {
                mods[i].OnCommandRan(command);
            }
        }

        /// <summary>
        /// Calls this method on all mods, also calls OnFirstPersonMoverSpawned if the passed character is a FirstPersonMover
        /// </summary>
        /// <param name="me"></param>
        protected internal override void OnCharacterSpawned(Character me)
        {
            List<Mod> mods = ModsManager.Instance.GetAllLoadedActiveMods();
            for (int i = 0; i < mods.Count; i++)
            {
                mods[i].OnCharacterSpawned(me);
            }
        }

        /// <summary>
        /// Calls this method on all mods, also calls OnFirstPersonMoverUpdate if the passed character is a firstpersonmover
        /// </summary>
        /// <param name="me"></param>
        protected internal override void OnCharacterUpdate(Character me)
        {
            List<Mod> mods = ModsManager.Instance.GetAllLoadedActiveMods();
            for (int i = 0; i < mods.Count; i++)
            {
                mods[i].OnCharacterUpdate(me);
            }
        }

        /// <summary>
        /// Calls this method on all mods
        /// </summary>
        /// <param name="killedCharacter"></param>
        /// <param name="killerCharacter"></param>
        /// <param name="damageSourceType"></param>
        /// <param name="attackID"></param>
        protected internal override void OnCharacterKilled(Character killedCharacter, Character killerCharacter, DamageSourceType damageSourceType, int attackID)
        {
            List<Mod> mods = ModsManager.Instance.GetAllLoadedActiveMods();
            for (int i = 0; i < mods.Count; i++)
            {
                mods[i].OnCharacterKilled(killedCharacter, killerCharacter, damageSourceType, attackID);
            }
        }

        /// <summary>
        /// Calls this method on all mods
        /// </summary>
        protected internal override void OnModDeactivated()
        {
            List<Mod> mods = ModsManager.Instance.GetAllLoadedActiveMods();
            for (int i = 0; i < mods.Count; i++)
            {
                mods[i].OnModDeactivated();
            }
        }

        /// <summary>
        /// Gets the response from this from all loaded mods, and uses the or operator on all of them, then returns
        /// </summary>
        /// <returns></returns>
        protected internal override bool ShouldCursorBeEnabled() // if any mod tells the game that the cursor should be enabled, it will be
        {
            List<Mod> mods = ModsManager.Instance.GetAllLoadedActiveMods();
            foreach (Mod mod in mods)
            {
                if (mod.ShouldCursorBeEnabled())
                    return true;
            }

            return Generic2ButtonDialogue.IsWindowOpen;
        }

        /// <summary>
        /// Calls this method on all mods
        /// </summary>
        protected internal override void GlobalUpdate()
        {
            List<Mod> mods = ModsManager.Instance.GetAllLoadedActiveMods();
            for (int i = 0; i < mods.Count; i++)
            {
                mods[i].GlobalUpdate();
            }
        }

        /// <summary>
        /// Calls this method on all mods
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        protected internal override UnityEngine.Object OnResourcesLoad(string path)
        {
            List<Mod> mods = ModsManager.Instance.GetAllLoadedActiveMods();
            for (int i = 0; i < mods.Count; i++)
            {
                UnityEngine.Object obj = mods[i].OnResourcesLoad(path);
                if (obj != null)
                    return obj;
            }

            return null;
        }

        /// <summary>
        /// Calls this method on all mods
        /// </summary>
        /// <param name="newLanguageID"></param>
        /// <param name="localizationDictionary"></param>
        protected internal override void OnLanguageChanged(string newLanguageID, Dictionary<string, string> localizationDictionary)
        {
            List<Mod> mods = ModsManager.Instance.GetAllLoadedActiveMods();
            for (int i = 0; i < mods.Count; i++)
            {
                mods[i].OnLanguageChanged(newLanguageID, localizationDictionary);
            }
        }

        /// <summary>
        /// Calls this method on all mods
        /// </summary>
        protected internal override void OnClientConnectedToServer()
        {
            List<Mod> mods = ModsManager.Instance.GetAllLoadedActiveMods();
            for (int i = 0; i < mods.Count; i++)
            {
                mods[i].OnClientConnectedToServer();
            }
        }

        /// <summary>
        /// Calls this method on all mods
        /// </summary>
        protected internal override void OnClientDisconnectedFromServer()
        {
            List<Mod> mods = ModsManager.Instance.GetAllLoadedActiveMods();
            for (int i = 0; i < mods.Count; i++)
            {
                mods[i].OnClientDisconnectedFromServer();
            }
        }
    }
}

