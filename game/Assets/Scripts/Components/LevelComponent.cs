using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Utils;

namespace DevGate
{
    public class LevelComponent : MonoBehaviour
    {
        private Lifetime.Definition _definition;

        public Camera Camera;
        public Animator StartLevelAnimator;
        public LevelSettingsScriptableObject Settings;
        public InputControllerComponent InputController;
        public GameHudComponent HudComponent;
        public LevelState State;
        public SpawnController Spawn;

        public Lifetime Lifetime { get { return _definition.Lifetime; } }

        public void StartLevel()
        {
            HudComponent.Init(this, Lifetime);

            StartLevelAnimator.SetTrigger("Start");
        }

        private void Awake()
        {
            _definition = Lifetime.Define(GameContext.LevelController.LeveLifetime);

            InputController = new InputControllerComponent();
            InputController.Init(Lifetime);
            State = new LevelState();
            State.Init(Lifetime);

            Spawn = new SpawnController(this, Settings);
            Spawn.Init();
        }

        private void OnDestroy()
        {
            _definition.Terminate();
        }
    }
}
