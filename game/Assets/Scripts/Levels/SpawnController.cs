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
        private readonly List<Vector3> _spawnPoints;

        public SpawnController(LevelComponent levelComponent, LevelSettingsScriptableObject settings)
        {
            _levelComponent = levelComponent;
            _settings = settings;

            _spawnPoints = new List<Vector3>();
        }

        public void Add(Vector3 point)
        {
            _spawnPoints.Add(point);
        }

        public void Init()
        {
            // find points
            var position = _levelComponent.Environment.transform.position;
            var mesh = _levelComponent.Environment.Mesh.sharedMesh;
            foreach (var vertex in mesh.vertices)
            {
                var point = position + vertex;
                Add(point);
            }
        }
    }
}
