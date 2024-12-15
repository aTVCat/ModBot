﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace ModLibrary
{
    /// <summary>
    /// Manages the input of a custom KeyCode value in the modded options page
    /// </summary>
    public class KeyCodeInput : MonoBehaviour
    {
        Text _keyDisplay;

        Action<KeyCode> _onChange;
        KeyCode _selectedKey;
        /// <summary>
        /// Gets or sets the current KeyCode
        /// </summary>
        public KeyCode SelectedKey
        {
            get => _selectedKey;
            set
            {
                _selectedKey = value;

                if (_onChange != null)
                    _onChange(value);

                _keyDisplay.text = value.ToString();
            }
        }

        internal void Init(KeyCode defualtValue, Action<KeyCode> onChange)
        {
            ModdedObject moddedObject = GetComponent<ModdedObject>();
            _keyDisplay = moddedObject.GetObject<Text>(0);
            Button switchInputButton = moddedObject.GetObject<Button>(1);
            switchInputButton.onClick.AddListener(AskForNewKey);

            SelectedKey = defualtValue;
            _onChange = onChange;
        }

        /// <summary>
        /// Asks the user to input a new key, if the user does not do so in 4 seconds aborts and does nothing
        /// </summary>
        public void AskForNewKey()
        {
            StartCoroutine(askForNewKey());
        }

        IEnumerator askForNewKey()
        {
            _keyDisplay.text = LocalizationManager.Instance.GetTranslatedString("request_new_key");
            float maxTime = Time.time + 4f;
            yield return new WaitUntil(delegate
            {
                return Input.anyKeyDown || Time.time >= maxTime;
            });

            KeyCode? selectedKeyCode = null;
            foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(keyCode))
                {
                    selectedKeyCode = keyCode;
                    break;
                }
            }

            if (!selectedKeyCode.HasValue)
            {
                SelectedKey = SelectedKey;
                yield break;
            }

            SelectedKey = selectedKeyCode.Value;

        }

    }
}
