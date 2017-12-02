using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;

namespace DevGate
{
    public class GoatSpawnController
    {
        private readonly LevelComponent _levelComponent;
        private readonly LevelSettingsScriptableObject _settings;
        private readonly Vector3[] _spawnPoints;
        private readonly List<SpawnComponent> _activeSpawns;
        private readonly List<PoolFactory<SpawnComponent>> _factories;
        private readonly System.Random _random;
        private int _lastSpawnIndex;

        public GoatSpawnController(LevelComponent levelComponent, LevelSettingsScriptableObject settings)
        {
            _levelComponent = levelComponent;
            _settings = settings;

            _random = new System.Random();
            _spawnPoints = settings.GoatSpownPoints;
            _activeSpawns = new List<SpawnComponent>();


        }

        public void Init() { }

        public void Start()
        {
            GameContext.DelayCall(_levelComponent.Lifetime, _settings.DelaySpawn, StartListener);
        }

        private void StartListener()
        {
            GameContext.SubscribeOnUpdate(_levelComponent.Lifetime, (deltaTime) =>
            {
                foreach (var activeSpawn in _activeSpawns)
                {
                    if (_settings.GoatMinY < activeSpawn.transform.position.y)
                        activeSpawn.transform.Translate(new Vector3(0, -_settings.GoatSpawnSpeed * Time.deltaTime, 0));
                }
            });
            SpawnObject();
        }

        private void SpawnObject()
        {
            var def = Lifetime.Define(_levelComponent.Lifetime);
            var point = _spawnPoints[_random.Next(_spawnPoints.Length)];
            var spawn = GameObject.Instantiate(_settings.GoatSpawnComponent);
            spawn.transform.position = point;
            spawn.transform.SetParent(_levelComponent.ActiveSpawnTransform, true);
            _activeSpawns.Add(spawn);
        }

    }
}
