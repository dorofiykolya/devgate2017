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
        private Transform _poolTransform;
        private Transform _bulletTransform;
        private Transform _activeSpawnTransform;
        private Transform _runeTransform;
        private Transform _effectTransform;

        public Camera Camera;
        public Animator StartLevelAnimator;
        public LevelSettingsScriptableObject Settings;
        public InputControllerComponent InputController;
        public GameHudComponent HudComponent;
        public LevelState State;
        public SpawnController Spawn;
        public GoatSpawnController GoatSpawn;
        public LevelEnvironmentComponent Environment;
        public ShootController ShootController;
        public BoatControllerComponent BoatController;
        public CameraShakeComponent ShakeComponent;
        public RuneRequirementController RuneRequirementController;
        public ExplosionController ExplosionController;
        public ExplosionController SpawnEffectController;

        public Transform[] RunesPositions;

        public Lifetime Lifetime { get { return _definition.Lifetime; } }
        public Transform BulletTransform { get { return _bulletTransform; } }
        public Transform ActiveSpawnTransform { get { return _activeSpawnTransform; } }
        public Transform RuneTransform { get { return _runeTransform; } }
        public Transform EffectTransform { get { return _effectTransform; } }

        public void StartLevel()
        {
            HudComponent.Init(this, Lifetime);

            StartLevelAnimator.SetTrigger("Start");
            Spawn.Start();
            GoatSpawn.Start();
            RuneRequirementController.Start();
        }

        public void ShakeCamera()
        {
            ShakeComponent.Shake();
        }

        public void ToPool(Transform value)
        {
            value.SetParent(_poolTransform);
        }

        private void Awake()
        {
            if (!GameContext.Initialized) return;

            _definition = Lifetime.Define(GameContext.LevelController.LeveLifetime);

            _bulletTransform = new GameObject("Bullets").transform;
            _bulletTransform.SetParent(transform, false);
            _poolTransform = new GameObject("Pool").transform;
            _poolTransform.SetParent(transform, false);
            _poolTransform.gameObject.SetActive(false);
            _activeSpawnTransform = new GameObject("ActiveSpawn").transform;
            _activeSpawnTransform.SetParent(transform, false);
            _runeTransform = new GameObject("Runes").transform;
            _runeTransform.SetParent(transform, false);
            _effectTransform = new GameObject("Effects").transform;
            _effectTransform.SetParent(transform, false);

            InputController = new InputControllerComponent();
            InputController.Init(Lifetime);

            State = new LevelState();
            State.Init(Lifetime);

            HudComponent.Init(this, Lifetime);

            Spawn = new SpawnController(this, Settings);
            Spawn.Init();

            GoatSpawn = new GoatSpawnController(this, Settings);
            GoatSpawn.Init();

            ShootController = new ShootController(this);

            RuneRequirementController = new RuneRequirementController(this);
            ExplosionController = new ExplosionController(this, Settings.ExplosionEffectPrefab);
            SpawnEffectController = new ExplosionController(this, Settings.SpawnEffectPrefab);

            //GameContext.DelayCall(1f, StartLevel);
        }

        private void OnDestroy()
        {
            if (_definition != null)
                _definition.Terminate();
        }
    }
}
