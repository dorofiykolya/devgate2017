using System.Collections;
using System.Collections.Generic;
using DevGate;
using UnityEngine;
using UnityEngine.UI;

namespace DevGate
{
    public class GameUIController : MonoBehaviour
    {

        [SerializeField] private Slider _powerSlider;

        [SerializeField] private InputControllerComponent _inputController;


        void Update()
        {
            _powerSlider.value = _inputController.CurrentPower / 100f;
        }
    }
}
