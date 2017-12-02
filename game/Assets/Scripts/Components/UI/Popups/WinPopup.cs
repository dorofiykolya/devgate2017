using System;
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
        }

        private void Start()
        {
            Show();
        }

        public void OnReplayClick()
        {
            Game.Instance.Restart();
            Hide();
        }

        public void OnNextClick()
        {
            //TODO
            Hide();
        }
    }
}
