using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DevGate
{
    class UIStartGameComponent : MonoBehaviour
    {
        private void Awake()
        {
            
        }

        public void StartLevel()
        {
            GetComponent<LevelComponent>().StartLevel();
        }
    }
}
