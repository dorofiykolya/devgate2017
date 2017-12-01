using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DevGate
{
    public class SpawnController
    {
        private readonly LevelComponent _levelComponent;
        private readonly LevelSettingsScriptableObject _settings;
        private readonly List<Transform> _spawnPoints;

        public SpawnController(LevelComponent levelComponent, LevelSettingsScriptableObject settings)
        {
            _levelComponent = levelComponent;
            _settings = settings;

            _spawnPoints = new List<Transform>();
        }

        public void Add(Transform transform)
        {
            _spawnPoints.Add(transform);
        }

        public void Init()
        {
            // find points
        }
    }
}
