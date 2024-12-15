﻿using ModLibrary;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace InternalModBot
{
    /// <summary>
    /// Used by Mod-Bot to reprecent a KeyCode item in a modded options window
    /// </summary>
    public class ModdedOptionKeyCodeItem : ModdedOptionPageItem
    {
        /// <summary>
        /// The default keycode
        /// </summary>
        public KeyCode DefaultValue;

        /// <summary>
        /// called when the KeyCodeInput item is created
        /// </summary>
        public Action<KeyCodeInput> OnCreate;
        /// <summary>
        /// Called when the keycode is changed
        /// </summary>
        public Action<KeyCode> OnChange;

        /// <summary>
        /// Places the page item in the page
        /// </summary>
        /// <param name="holder"></param>
        /// <param name="owner"></param>
        public override void CreatePageItem(GameObject holder, Mod owner)
        {
            KeyCodeInput keyCodeInput = InternalAssetBundleReferences.ModBot.InstantiateObject("CustomKeyCodeInput").AddComponent<KeyCodeInput>();
            keyCodeInput.transform.SetParent(holder.transform, false);
            keyCodeInput.Init(DefaultValue, delegate (KeyCode keyCode)
            {
                OptionsSaver.SetSetting(owner, SaveID, (int)keyCode, true);

                if (OnChange != null)
                    OnChange(keyCode);
            });

            object loadedValue = OptionsSaver.LoadSetting(owner, SaveID);
            if (loadedValue != null && loadedValue is int intValue && intValue != (int)DefaultValue)
                keyCodeInput.SelectedKey = (KeyCode)intValue;

            keyCodeInput.GetComponent<ModdedObject>().GetObject<Text>(2).text = DisplayName;

            applyCustomRect(keyCodeInput.gameObject);

            if (OnCreate != null)
                OnCreate(keyCodeInput);
        }

    }
}
