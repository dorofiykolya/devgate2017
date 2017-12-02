using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Utils;

namespace DevGate
{
    public class BoatRocketComponent : MonoBehaviour
    {
        private Lifetime.Definition _definition;

        private void OnEnable()
        {
            _definition = Lifetime.Define(GameContext.Lifetime);
        }

        private void OnDisable()
        {
            _definition.Terminate();
        }

        public void Go()
        {
            GameContext.SubscribeOnUpdate(_definition.Lifetime, OnUpdate);
        }

        private void OnUpdate(float deltaTime)
        {
            transform.Translate( Vector3.back * GameContext.LevelController.Current.Settings.RocketSpeed * Time.deltaTime);
        }

        public void OnTriggerStay(Collider collider)
        {
            var target = collider.gameObject.GetComponent<SpawnComponent>();
            if (target != null && !target.Hit)
            {
                target.Hit = true;
                GameContext.LevelController.Current.State.UpdateScore(target.RewardScore);
            }
        }
    }
}
