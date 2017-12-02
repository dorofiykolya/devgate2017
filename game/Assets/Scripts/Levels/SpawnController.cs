using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Utils;

namespace DevGate
{
    public class SpawnController
    {
        private readonly LevelComponent _levelComponent;
        private readonly LevelSettingsScriptableObject _settings;
        private readonly List<Vector3> _spawnPoints;
        private readonly List<ActiveSpawn> _activeSpawns;
        private readonly List<PoolFactory<SpawnComponent>> _factories;
        private readonly System.Random _random;
        private int _lastSpawnIndex;

        public SpawnController(LevelComponent levelComponent, LevelSettingsScriptableObject settings)
        {
            _levelComponent = levelComponent;
            _settings = settings;

            _random = new System.Random();
            _spawnPoints = new List<Vector3>();
            _activeSpawns = new List<ActiveSpawn>();
            _factories = new List<PoolFactory<SpawnComponent>>();

            foreach (var component in settings.SpawnComponents)
            {
                var factory = new PoolFactory<SpawnComponent>(() => GameObject.Instantiate<SpawnComponent>(component));
                _factories.Add(factory);
            }
        }

        public void AddSpawnPoint(Vector3 point)
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
                AddSpawnPoint(point);
            }
        }

        public void Start()
        {
            GameContext.DelayCall(_levelComponent.Lifetime, _settings.DelaySpawn, StartListener);
        }

        private void ToPool(Transform transform)
        {
            _levelComponent.ToPool(transform);
        }

        private void StartListener()
        {
            GameContext.SubscribeOnUpdate(_levelComponent.Lifetime, (deltaTime) =>
            {
                var toRemove = ListPool<ActiveSpawn>.Pop();
                foreach (var activeSpawn in _activeSpawns)
                {
                    if (activeSpawn.Spawn.Hit)
                    {
                        if (!activeSpawn.BlockRemove)
                        {
                            activeSpawn.Spawn.SubscribeOnPlayDestroyComplete(_levelComponent.Lifetime, () =>
                            {
                                _activeSpawns.Remove(activeSpawn);
                                activeSpawn.BlockRemove = false;
                            });
                            _levelComponent.ExplosionController.Explosion(activeSpawn.Spawn.transform.position);
                            activeSpawn.BlockRemove = true;
                            GameContext.StartCoroutine(activeSpawn.Spawn.PlayDestroy());
                        }
                    }
                    else
                    {
                        var trans = activeSpawn.Spawn.transform;
                        activeSpawn.CurrentVelocity += activeSpawn.Velocity * Time.deltaTime;
                        var speed = activeSpawn.Speed + activeSpawn.CurrentVelocity;
                        trans.Translate(new Vector3(0, speed * Time.deltaTime, 0));

                        if (trans.position.y > _settings.SpawnObjectMaxY)
                        {
                            toRemove.Add(activeSpawn);
                        }
                    }
                }
                foreach (var activeSpawn in toRemove)
                {
                    if (activeSpawn.Spawn.Type == SpawnType.Trash)
                    {
                        _levelComponent.State.UpdateLife(-1);
                    }
                    if (!activeSpawn.BlockRemove)
                    {
                        activeSpawn.Lifetime.Terminate();
                    }
                }
                ListPool.Push(toRemove);
            });
            GameContext.StartCoroutine(_levelComponent.Lifetime, SpawnObjects());
        }

        private IEnumerator SpawnObjects()
        {
            while (true)
            {
                var def = Lifetime.Define(_levelComponent.Lifetime);

                int index = 0;
                var i = 0;
                while (i++ < 3)
                {
                    index = _random.Next(_factories.Count);
                    if (index != _lastSpawnIndex) break;
                    _lastSpawnIndex = index;
                }

                var factory = _factories[index];
                var spawn = factory.Pop();
                spawn.transform.SetParent(_levelComponent.ActiveSpawnTransform);

                var point = _spawnPoints[_random.Next(_spawnPoints.Count)];
                spawn.transform.position = point;

                var active = new ActiveSpawn
                {
                    Spawn = spawn,
                    Lifetime = def,
                    Speed = _settings.SpawnSpeed,
                    Velocity = _settings.SpawnVelocity
                };

                _activeSpawns.Add(active);

                def.Lifetime.AddAction(() =>
                {
                    spawn.Hit = false;
                    factory.Push(spawn);
                    ToPool(spawn.transform);
                    _activeSpawns.Remove(active);
                });
                yield return new WaitForSeconds(_settings.SpawnTime);
            }
        }

        private class ActiveSpawn
        {
            public SpawnComponent Spawn;
            public Lifetime.Definition Lifetime;

            public bool BlockRemove;
            public float Speed;
            public float Velocity;
            public float CurrentVelocity;
        }
    }
}
