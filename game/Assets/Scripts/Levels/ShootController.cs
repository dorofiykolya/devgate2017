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

        private bool _lastShootLeft;

        public ShootController(LevelComponent level)
        {
            _level = level;

            _leftSpawn = level.BoatController.Left;
            _rightSpawn = level.BoatController.Right;

            _factories = new List<PoolFactory<BoatRocketComponent>>();

            foreach (var rocket in level.Settings.Rockets)
            {
                var factory = new PoolFactory<BoatRocketComponent>(() => GameObject.Instantiate(rocket));
                _factories.Add(factory);
            }
        }

        public void Shoot()
        {
            _lastShootLeft = !_lastShootLeft;
            Transform currentSpawn = _lastShootLeft ? _rightSpawn : _leftSpawn;

            var bullet = _level.Settings.Rockets[0];
        }

        public void ToPool(BoatRocketComponent component)
        {
            _level.ToPool(component.transform);
        }
    }
}
