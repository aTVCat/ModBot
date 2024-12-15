﻿using InternalModBot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace ModLibrary
{
    /// <summary>
    /// Contains information about an asset bundle with all the assets asynchronously cached
    /// </summary>
    public class AssetBundleInfo
    {
        static Dictionary<string, AssetBundleInfo> _cachedAssetBundleInfos = new Dictionary<string, AssetBundleInfo>();

        string _dictionaryKey;
        AssetBundle _assetBundle;
        Dictionary<string, UnityEngine.Object> _cachedObjects;

        string assetBundleFilePath => InternalUtils.GetSubdomain(Application.dataPath) + _dictionaryKey;

        bool _isCachingAssets;

        /// <summary>
        /// Unloads the AssetBundle when this object is destroyed
        /// </summary>
        ~AssetBundleInfo()
        {
            unloadAssetBundle();
            clearObjectCache(true);
        }

        private AssetBundleInfo(string cacheDictionaryKey)
        {
            _dictionaryKey = cacheDictionaryKey;
            _cachedObjects = new Dictionary<string, UnityEngine.Object>();

            loadAssetBundle();
        }

        internal static AssetBundleInfo CreateOrGetCached(string key)
        {
            if (_cachedAssetBundleInfos.ContainsKey(key))
                return _cachedAssetBundleInfos[key];

            return createAssetBundle(key);
        }

        static AssetBundleInfo createAssetBundle(string key)
        {
            string filePath = InternalUtils.GetSubdomain(Application.dataPath) + key;

            if (!File.Exists(filePath))
                throw new FileNotFoundException("Could not find asset bundle file", filePath);

            AssetBundleInfo assetBundleInfo = new AssetBundleInfo(key);
            _cachedAssetBundleInfos.Add(key, assetBundleInfo);

            return assetBundleInfo;
        }

        internal static void TryCacheAssetBundle(string key)
        {
            if (!_cachedAssetBundleInfos.ContainsKey(key))
                createAssetBundle(key);
        }

        internal static void ClearCache()
        {
            foreach (AssetBundleInfo assetBundleInfo in _cachedAssetBundleInfos.Values)
            {
                assetBundleInfo.unloadAssetBundle();
                assetBundleInfo.clearObjectCache();
            }

            _cachedAssetBundleInfos.Clear();
        }

        void clearObjectCache(bool removeInstanceFromCache = false)
        {
            if (_cachedObjects != null)
            {
                _cachedObjects.Clear();
                _cachedObjects = null;
            }

            if (removeInstanceFromCache)
                _cachedAssetBundleInfos.Remove(_dictionaryKey);
        }

        void loadAssetBundle()
        {
            if (_assetBundle == null)
            {
                _assetBundle = AssetBundle.LoadFromFile(assetBundleFilePath);
                startCachingAllAssets();
            }
        }

        void unloadAssetBundle()
        {
            if (_assetBundle != null)
            {
                _assetBundle.Unload(false);
                _assetBundle = null;
            }
        }

        void startCachingAllAssets()
        {
            StaticCoroutineRunner.StartStaticCoroutine(cacheAllAssets());
        }

        IEnumerator cacheAllAssets()
        {
            if (_assetBundle == null)
                yield break;

            _isCachingAssets = true;

            AssetBundleRequest allAssetsRequest = _assetBundle.LoadAllAssetsAsync();
            yield return allAssetsRequest;

            if (_assetBundle == null || _cachedObjects == null)
            {
                _isCachingAssets = false;
                yield break;
            }

            foreach (UnityEngine.Object asset in allAssetsRequest.allAssets)
            {
                string name = asset.name;

                if (!_cachedObjects.ContainsKey(name)) // This object might have already been added to the cache from GetObject
                    _cachedObjects.Add(name, asset);
            }

            unloadAssetBundle();
            _isCachingAssets = false;
        }

        /// <summary>
        /// Loads an object from the <see cref="AssetBundle"/>. Note: This will most likely be a prefab, if you want to instantiate the object you might want to use <see cref="InstantiateObject{T}(string)"/> instead
        /// </summary>
        /// <typeparam name="T">The type of object to load</typeparam>
        /// <param name="objectName">The name of the object to load</param>
        /// <returns>The object loaded from the asset bundle</returns>
        public T GetObject<T>(string objectName) where T : UnityEngine.Object
        {
            T asset;
            if (_cachedObjects.TryGetValue(objectName, out UnityEngine.Object obj))
            {
                asset = obj as T;

                if (asset == null || !asset)
                {
                    _cachedObjects.Clear();
                    startCachingAllAssets();
                    return GetObject<T>(objectName);
                }
            }
            else if (_assetBundle != null)
            {
                asset = _assetBundle.LoadAsset<T>(objectName);

                if (asset != null)
                    _cachedObjects.Add(objectName, asset);

                // If we are not caching the assets but the asset is missing from the cache, start caching the assets if we aren't already doing so
                if (!_isCachingAssets)
                    startCachingAllAssets();
            }
            else
            {
                asset = null;
            }

            if (asset == null)
            {
                if (_assetBundle == null)
                {
                    // Reload asset bundle and retry loading the asset
                    loadAssetBundle();
                    return GetObject<T>(objectName);
                }
                else
                {
                    throw new Exception("Object \"" + objectName + "\" of type \"" + typeof(T) + "\" was not found in asset bundle \"" + _assetBundle.name + "\"");
                }
            }

            return asset;
        }

        /// <summary>
        /// Loads a <see cref="GameObject"/> from the <see cref="AssetBundle"/>. Note: This will most likely be a prefab, if you want to instantiate the <see cref="GameObject"/> you might want to use <see cref="InstantiateObject(string)"/> instead
        /// </summary>
        /// <param name="objectName">The name of the <see cref="GameObject"/> to load</param>
        /// <returns>The <see cref="GameObject"/> loaded from the <see cref="AssetBundle"/></returns>
        public GameObject GetObject(string objectName)
        {
            return GetObject<GameObject>(objectName);
        }

        /// <summary>
        /// Instantiates an object of type <typeparamref name="T"/> from the <see cref="AssetBundle"/>
        /// </summary>
        /// <typeparam name="T">The type of the object to instantiate</typeparam>
        /// <param name="objectName">The name of the object to instantiate</param>
        /// <returns>The instantiated object</returns>
        public T InstantiateObject<T>(string objectName) where T : UnityEngine.Object
        {
            return UnityEngine.Object.Instantiate(GetObject<T>(objectName));
        }

        /// <summary>
        /// Instantiates a <see cref="GameObject"/> from the <see cref="AssetBundle"/>
        /// </summary>
        /// <param name="objectName">The name of the <see cref="GameObject"/> to instantiate</param>
        /// <returns>The instantiated <see cref="GameObject"/></returns>
        public GameObject InstantiateObject(string objectName)
        {
            return InstantiateObject<GameObject>(objectName);
        }
    }
}
