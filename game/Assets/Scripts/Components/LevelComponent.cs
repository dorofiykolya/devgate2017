using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DevGate
{
    public class LevelComponent : MonoBehaviour
    {
        public Camera Camera;
        public Animator StartLevelAnimator;
        public LevelSettingsScriptableObject Settings;
        public InputControllerComponent InputController;

        public void StartLevel()
        {
            StartLevelAnimator.SetTrigger("Start");

            InputController = new InputControllerComponent();
            InputController.Init();
        }
    }
}
