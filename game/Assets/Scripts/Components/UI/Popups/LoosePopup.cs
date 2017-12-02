using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DevGate
{
    public class LoosePopup : PopupContent
    {
        public static void ShowPopup()
        {
            var go = Resources.Load<GameObject>("Prefabs/Popups/LoosePopup");
            Instantiate<GameObject>(go);
        }

        void Awake()
        {
            OnAwake();
            PlaySound("Audio/loose");
        }

        private void Start()
        {
            Show();
        }

        public void OnReplayClick()
        {
            StartCoroutine(WaitAndRestart());
        }

        IEnumerator WaitAndRestart()
        {
            yield return null;
            Game.Instance.Restart();
        }
    }
}
