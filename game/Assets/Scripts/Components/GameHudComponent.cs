using System.Collections;
using System.Collections.Generic;
using DevGate;
using UnityEngine;
using UnityEngine.UI;
using Utils;

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

        [Space(5)]
        [SerializeField]
        private Sprite pauseSprite;
        [SerializeField]
        private Sprite playSprite;
        [SerializeField]
        private Image playPauseImage;

        public void Init(LevelComponent level, Lifetime lifetime)
        {
            level.InputController.SubscribeOnPowerChange(lifetime, UpdatePower);
           // level.State.SubscribeOnStateChanged(lifetime, UpdateLevelState);
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

        public void OnPlayPauseClick()
        {
            GameContext.LevelController.Current.State.IsPause = !GameContext.LevelController.Current.State.IsPause;
            playPauseImage.sprite = GameContext.LevelController.Current.State.IsPause ? playSprite : pauseSprite;
        }
    }
}
