using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace DevGate
{
    public class WinPopup : PopupContent
    {
        [SerializeField]
        private Text _scoreText;


        public static void ShowPopup()
        {
            var go = Resources.Load<GameObject>("Prefabs/Popups/WinPopup");
            Instantiate<GameObject>(go);
        }

        void Awake()
        {
            OnAwake();
            _scoreText.text = GameContext.LevelController.Current.State.Score.ToString();
            PlaySound("Audio/win");
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

        public void OnNextClick()
        {
            //TODO
            Hide();
        }
    }
}
