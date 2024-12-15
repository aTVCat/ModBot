﻿using ModLibrary;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace InternalModBot
{
    /// <summary>
    /// Used by Mod-Bot to reprecent label items on modded option pages
    /// </summary>
    public class ModdedOptionLabelItem : ModdedOptionPageItem
    {
        /// <summary>
        /// Called when the label is created
        /// </summary>
        public Action<Text> OnCreate;

        /// <summary>
        /// Places the page item in the page
        /// </summary>
        /// <param name="holder"></param>
        /// <param name="owner"></param>
        public override void CreatePageItem(GameObject holder, Mod owner)
        {
            GameObject spawnedPrefab = InternalAssetBundleReferences.ModBot.InstantiateObject("Label");
            spawnedPrefab.transform.SetParent(holder.transform, false);

            ModdedObject spawnedModdedObject = spawnedPrefab.GetComponent<ModdedObject>();
            Text text = spawnedModdedObject.GetObject<Text>(0);
            text.text = DisplayName;

            applyCustomRect(spawnedPrefab);

            if (OnCreate != null)
                OnCreate(text);
        }

    }
}
