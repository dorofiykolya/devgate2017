using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DevGate
{
    class UIStartGameComponent : MonoBehaviour
    {
        public GameObject Text;

        private void Awake()
        {

        }

        private void Update()
        {
            if (Input.GetMouseButton(0) || Input.touchCount != 0)
            {
                StartLevel();
                enabled = false;
                Destroy(this);
            }
        }

        public void StartLevel()
        {
            GetComponent<LevelComponent>().StartLevel();
            Destroy(Text);
        }
    }
}
