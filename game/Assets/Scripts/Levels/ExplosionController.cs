using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Utils;

namespace DevGate
{
    public class ExplosionController
    {
        private readonly LevelComponent _level;
        private readonly PoolFactory<ExplosionEffectComponent> _factory;

        public ExplosionController(LevelComponent level)
        {
            _level = level;
            _factory = new PoolFactory<ExplosionEffectComponent>(() => GameObject.Instantiate(level.Settings.ExplosionEffectPrefab));
        }

        public void Explosion(Vector3 position)
        {
            var def = Lifetime.Define(_level.Lifetime);
            var exp = _factory.Pop();
            exp.transform.SetParent(_level.EffectTransform, false);
            exp.transform.position = position;
            exp.Play();
            def.Lifetime.AddAction(() =>
            {
                _factory.Push(exp);
                _level.ToPool(exp.transform);
            });
            exp.SubscribeOnComplete(def.Lifetime, () =>
            {
                def.Terminate();
            });
        }
    }
}
