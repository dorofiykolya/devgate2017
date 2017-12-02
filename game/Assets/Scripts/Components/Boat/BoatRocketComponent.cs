using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DevGate
{
    public class BoatRocketComponent : MonoBehaviour
    {
        float speed = 20;

        public void Go()
        {
            GameContext.SubscribeOnUpdate(GameContext.Lifetime, OnUpdate);
            transform.SetParent(null);
        }

        private void OnUpdate(float deltaTime)
        {
            transform.Translate( Vector3.back * speed * Time.deltaTime);
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
