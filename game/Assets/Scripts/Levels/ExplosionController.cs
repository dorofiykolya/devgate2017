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

        public ExplosionController(LevelComponent level, ExplosionEffectComponent prefab)
        {
            _level = level;
            _factory = new PoolFactory<ExplosionEffectComponent>(() => GameObject.Instantiate(prefab));
            var instances = ListPool<ExplosionEffectComponent>.Pop();
            for (int i = 0; i < 5; i++)
            {
                instances.Add(_factory.Pop());
            }
            foreach (var instance in instances)
            {
                _level.ToPool(instance.transform);
                _factory.Push(instance);
            }
            ListPool.Push(instances);
        }

        public void Explosion(Vector3 position)
        {
            var def = Lifetime.Define(_level.Lifetime);
            var exp = _factory.Pop();
            exp.transform.SetParent(_level.EffectTransform, false);
            exp.transform.position = position;
            GameContext.DelayCall(def.Lifetime, 1.5f, exp.FireOnComplete);
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
