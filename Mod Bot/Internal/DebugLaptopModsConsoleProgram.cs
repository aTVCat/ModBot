using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace InternalModBot
{
    internal class DebugLaptopModsConsoleProgram : DebugLaptopProgram
    {
        private RectTransform _rectTransform;

        public ConsoleUI UIController;

        private void Start()
        {
            if (!hasStartedInLaptop()) return;

            UIController = base.gameObject.AddComponent<ConsoleUI>();
            UIController.Init(true);

            _rectTransform = base.transform as RectTransform;
            _rectTransform.pivot = Vector2.one * 0.5f;
            _rectTransform.anchorMax = Vector2.one;
            _rectTransform.anchorMin = Vector2.zero;
            _rectTransform.anchoredPosition = Vector2.zero;
            _rectTransform.sizeDelta = Vector2.zero;
        }

        private bool hasStartedInLaptop()
        {
            return base.transform.parent;
        }
    }
}
