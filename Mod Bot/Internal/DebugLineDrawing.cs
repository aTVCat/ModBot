﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InternalModBot
{
    /// <summary>
    /// Used by Mod-Bot to draw lines on screen
    /// </summary>
    internal class DebugLineDrawingManager : Singleton<DebugLineDrawingManager>
    {
        List<LineInfo> _linesToDraw = new List<LineInfo>();

        /// <summary>
        /// Adds a line to the lines to draw this frame
        /// </summary>
        /// <param name="info"></param>
        public void AddLine(LineInfo info)
        {
            _linesToDraw.Add(info);
        }

        void Update()
        {
            Camera main = Camera.main;
            if (main == null)
            {
                //todo: search for VRPlayerCharacter camera
                return;
            }
            if (main.GetComponent<DebugLineDrawer>() == null)
            {
                main.gameObject.AddComponent<DebugLineDrawer>();
            }

            StartCoroutine(runAtEndOfFrame());
        }

        IEnumerator runAtEndOfFrame()
        {
            yield return new WaitForEndOfFrame();

            for (int i = 0; i < _linesToDraw.Count; i++)
            {
                if (_linesToDraw[i].EndTime <= Time.unscaledTime)
                {
                    _linesToDraw.RemoveAt(i);
                    i--;
                }
            }

        }

        class DebugLineDrawer : MonoBehaviour
        {
            Material _lineMaterial;

            void Awake()
            {
                _lineMaterial = InternalAssetBundleReferences.ModBot.GetObject<Material>("Line");
            }

            void OnPostRender()
            {
                GL.Begin(GL.LINES);

                for (int i = 0; i < Instance._linesToDraw.Count; i++)
                {
                    _lineMaterial.SetPass(0);

                    LineInfo info = Instance._linesToDraw[i];
                    GL.Color(info.Color);
                    GL.Vertex(info.Point1);
                    GL.Vertex(info.Point2);
                }

                GL.End();
            }
        }
    }
}
