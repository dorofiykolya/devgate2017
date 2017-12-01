using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIController : MonoBehaviour
{

    [SerializeField]
    private Slider _powerSlider;

    [SerializeField]
    private InputController _inputController;

    
    void Update()
    {
        _powerSlider.value = _inputController.CurrentPower / 100f;
    }
}
