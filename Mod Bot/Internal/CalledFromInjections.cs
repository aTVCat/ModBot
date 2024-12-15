﻿using UnityEngine;

namespace InternalModBot
{
    /// <summary>
    /// Contains methods that get called from the game itself
    /// </summary>
    internal static class CalledFromInjections
    {
        /// <summary>
        /// Called from <see cref="UnityEngine.Resources.Load(string)"/>, <see cref="UnityEngine.Resources.Load{T}(string)"/> and <see cref="ResourceRequest.asset"/>
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static UnityEngine.Object FromResourcesLoad(string path)
        {
            if (ModsManager.Instance == null)
                return null;

            return ModsManager.Instance.PassOnMod.OnResourcesLoad(path);
        }
    }
}
