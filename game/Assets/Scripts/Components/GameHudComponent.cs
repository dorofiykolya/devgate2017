using System.Collections;
using System.Collections.Generic;
using DevGate;
using UnityEngine;
using UnityEngine.UI;

namespace DevGate
{
    public class GameHudComponent : MonoBehaviour
    {

        [SerializeField] private Slider _powerSlider;

        
        public void Init(LevelComponent level)
        {
            level.InputController.SubscribeOnPowerChange(GameContext.Lifetime, UpdatePower);
        }

        private void UpdatePower(float value)
        {
            _powerSlider.value = value / 100f;
        }
    }
}
