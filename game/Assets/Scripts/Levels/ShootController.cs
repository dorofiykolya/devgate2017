using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Utils;

namespace DevGate
{
    public class ShootController
    {
        private readonly LevelComponent _level;
        private Transform _leftSpawn;
        private Transform _rightSpawn;
        private List<PoolFactory<BoatRocketComponent>> _factories;
        private List<ActiveBullet> _activeBullets;

        private bool _lastShootLeft;

        private bool _canShoot = true;
        private AudioClip shootClip;

        public ShootController(LevelComponent level)
        {
            shootClip = Resources.Load<AudioClip>("Audio/torpedo");
            _level = level;

            _leftSpawn = level.BoatController.Left;
            _rightSpawn = level.BoatController.Right;

            _activeBullets = new List<ActiveBullet>();
            _factories = new List<PoolFactory<BoatRocketComponent>>();

            foreach (var rocket in level.Settings.Rockets)
            {
                var factory = new PoolFactory<BoatRocketComponent>(() => GameObject.Instantiate(rocket));
                _factories.Add(factory);
            }

            level.InputController.SubscribeOnShoot(GameContext.Lifetime, Shoot);

            GameContext.SubscribeOnUpdate(level.Lifetime, (deltaTime) =>
            {
                var toRemove = ListPool<ActiveBullet>.Pop();
                foreach (var bullet in _activeBullets)
                {
                    if (bullet.Rocket.transform.position.z <= _level.Settings.BulletMinZ)
                    {
                        toRemove.Add(bullet);
                    }
                }
                foreach (var bullet in toRemove)
                {
                    bullet.Lifetime.Terminate();
                }
                ListPool.Push(toRemove);
            });
        }

        public void Shoot()
        {
            if (!_canShoot) return;
            _canShoot = false;
            _lastShootLeft = !_lastShootLeft;
            Transform currentSpawn = _lastShootLeft ? _rightSpawn : _leftSpawn;

            var factory = _factories[0];
            var bulletGo = factory.Pop();
            var bt = bulletGo.transform;
            bt.SetParent(_level.BulletTransform, false);
            bt.position = currentSpawn.position;
            bt.rotation = currentSpawn.rotation;
            bulletGo.Go();

            var def = Lifetime.Define(_level.Lifetime);
            var bullet = new ActiveBullet
            {
                Rocket = bulletGo,
                Lifetime = def
            };
            _activeBullets.Add(bullet);

            def.Lifetime.AddAction(() =>
            {
                factory.Push(bulletGo);
                _level.ToPool(bulletGo.transform);
                _activeBullets.Remove(bullet);
            });
            Sound.Play(shootClip);

            _level.ShakeCamera();
            GameContext.DelayCall(_level.Settings.ShootDelay, () => _canShoot = true);
        }

        public void ToPool(BoatRocketComponent component)
        {
            _level.ToPool(component.transform);
        }

        private class ActiveBullet
        {
            public BoatRocketComponent Rocket;
            public Lifetime.Definition Lifetime;
        }
    }
}
