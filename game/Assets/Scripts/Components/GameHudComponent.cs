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
            GameContext.LevelController.Current.State.SubscribeOnStateChanged(GameContext.Lifetime, UpdateLevelState);
        }

        private void UpdatePower(float value)
        {
            _powerSlider.value = value / 100f;
        }
        private void UpdateLevelState(LevelState state)
        {
            _scoreText.text = state.Score.ToString();
            _lifeText.text = state.Life.ToString();
        }
    }
}
