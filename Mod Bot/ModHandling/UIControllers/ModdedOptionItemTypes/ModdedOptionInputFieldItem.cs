﻿using ModLibrary;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace InternalModBot
{
    /// <summary>
    /// Used by Mod-Bot to reprecent a InputField in a modded options page
    /// </summary>
    public class ModdedOptionInputFieldItem : ModdedOptionPageItem
    {
        /// <summary>
        /// The default value of the input field
        /// </summary>
        public string DefaultValue;

        /// <summary>
        /// Called when the InputField is spawned
        /// </summary>
        public Action<InputField> OnCreate;
        /// <summary>
        /// Called when the content of the input field is changed
        /// </summary>
        public Action<string> OnChange;

        /// <summary>
        /// Places the page item in the page
        /// </summary>
        /// <param name="holder"></param>
        /// <param name="owner"></param>
        public override void CreatePageItem(GameObject holder, Mod owner)
        {
            GameObject spawnedPrefab = InternalAssetBundleReferences.ModBot.InstantiateObject("InputField");
            spawnedPrefab.transform.SetParent(holder.transform, false);
            ModdedObject spawnedModdedObject = spawnedPrefab.GetComponent<ModdedObject>();
            spawnedModdedObject.GetObject<Text>(0).text = DisplayName;
            InputField inputField = spawnedModdedObject.GetObject<InputField>(1);
            inputField.text = DefaultValue;

            object loadedValue = OptionsSaver.LoadSetting(owner, SaveID);
            if (loadedValue != null && loadedValue is string stringValue)
                inputField.text = stringValue;

            if (OnChange != null)
                OnChange(inputField.text);

            inputField.onValueChanged.AddListener(delegate (string value)
            {
                OptionsSaver.SetSetting(owner, SaveID, value, true);

                if (OnChange != null)
                    OnChange(value);
            });

            applyCustomRect(spawnedPrefab);

            if (OnCreate != null)
                OnCreate(inputField);
        }

    }
}
