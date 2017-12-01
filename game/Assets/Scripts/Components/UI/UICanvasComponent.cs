using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DevGate
{
    class UICanvasComponent : MonoBehaviour
    {
        private void Awake()
        {
            if (EventSystem.current == null)
            {
                Instantiate(Resources.Load("Prefabs/EventSystem"));
            }
        }
    }
}
