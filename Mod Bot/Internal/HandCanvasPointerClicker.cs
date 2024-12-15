using Autohand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ModLibrary.Internal
{
    public class HandCanvasPointerClicker : MonoBehaviour
    {
        private HandCanvasPointer HandCanvasPointer;

        private void Update()
        {
            if (!HandCanvasPointer)
            {
                HandCanvasPointer = base.GetComponent<HandCanvasPointer>();
                if(!HandCanvasPointer)
                {
                    base.enabled = false;
                    return;
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                HandCanvasPointer.Press();
            }
            else if (Input.GetMouseButtonUp(0))
            {
                HandCanvasPointer.Release();
            }
        }
    }
}
