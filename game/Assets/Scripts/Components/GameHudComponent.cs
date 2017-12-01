using System.Collections;
using System.Collections.Generic;
using DevGate;
using UnityEngine;
using UnityEngine.UI;

namespace DevGate
{
    public class GameHudComponent : MonoBehaviour
    {
        [SerializeField]
        private Text _scoreText;

        [SerializeField]
        private Text _lifeText;

        [SerializeField]
        private Slider _powerSlider;


        public void Init()
        {
            GameContext.LevelController.Current.InputController.SubscribeOnPowerChange(GameContext.Lifetime, UpdatePower);
        }

        private void UpdatePower(float value)
        {
            _powerSlider.value = value / 100f;
        }
        private void UpdateScores(int value)
        {
            _scoreText.text = value.ToString();
        }
        private void UpdateLife(int value)
        {
            _lifeText.text = value.ToString();
        }
    }
}
