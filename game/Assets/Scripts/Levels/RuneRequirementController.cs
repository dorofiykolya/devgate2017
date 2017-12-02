using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Utils;

namespace DevGate
{
    public class RuneRequirementController
    {
        private readonly LevelComponent _level;
        private SpawnComponent[] _runes;
        private List<ActiveRune> _requirements;
        private List<PoolFactory<SpawnComponent>> _factories;

        public RuneRequirementController(LevelComponent level)
        {
            _level = level;
            _runes = _level.Settings.SpawnComponents.Where(s => s.Type == SpawnType.Rune).ToArray();
            _requirements = new List<ActiveRune>();
            _factories = new List<PoolFactory<SpawnComponent>>();
            foreach (var component in _runes)
            {
                _factories.Add(new PoolFactory<SpawnComponent>(() => GameObject.Instantiate<SpawnComponent>(component)));
            }
        }

        public SpawnComponent[] Runes { get { return _runes; } }

        public void Start()
        {
            SpawnRequirement();
        }

        public IEnumerable<SpawnComponent> Requirements { get { return _requirements.Select(s => s.Spawn); } }

        public void Hited(SpawnComponent spawn)
        {
            if (spawn.Type == SpawnType.Rune)
            {
                var obj = _requirements.Find(s => s.Spawn.RuneId == spawn.RuneId);
                if (obj != null)
                {
                    obj.Lifetime.Terminate();
                }
            }
        }

        public void SpawnRequirement()
        {
            if (_requirements.Count == 0)
            {
                for (int i = 0; i < 4; i++)
                {
                    var index = UnityEngine.Random.Range(0, _factories.Count);
                    var factory = _factories[index];
                    var spawn = factory.Pop();
                    spawn.transform.position = _level.RunesPositions[i].position;
                    spawn.transform.SetParent(_level.RuneTransform);
                    var def = Lifetime.Define(_level.Lifetime);

                    var activeReq = new ActiveRune();
                    activeReq.Spawn = spawn;
                    activeReq.Lifetime = def;

                    _requirements.Add(activeReq);

                    def.Lifetime.AddAction(() =>
                    {
                        factory.Push(spawn);
                        _level.ToPool(spawn.transform);
                        _requirements.Remove(activeReq);
                    });
                }
            }
        }

        private class ActiveRune
        {
            public SpawnComponent Spawn;
            public Lifetime.Definition Lifetime;
        }
    }
}
