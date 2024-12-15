﻿using ModLibrary;
using UnityEngine;

namespace InternalModBot
{
    /// <summary>
    /// Attaced to mod options windows to close the window when the user clicks escape
    /// </summary>
    internal class CloseModOptionsWindowOnEscapeKey : MonoBehaviour
    {
        ModOptionsWindowBuilder _owner;

        /// <summary>
        /// Sets the owner to the value passed
        /// </summary>
        /// <param name="owner"></param>
        public void Init(ModOptionsWindowBuilder owner)
        {
            _owner = owner;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _owner.CloseWindow();
                Destroy(this);
            }
        }
    }
}
