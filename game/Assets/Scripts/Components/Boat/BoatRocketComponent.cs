using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DevGate
{
    public class BoatRocketComponent : MonoBehaviour
    {
        float speed = 10;

        void Awake()
        {
            GameContext.SubscribeOnUpdate(GameContext.Lifetime, OnUpdate);
        }

        private void Start()
        {
            transform.SetParent(null);
        }

        private void OnUpdate(float deltaTime)
        {
            transform.Translate( Vector3.back * speed * Time.deltaTime);
        }

        public void OnTriggerEnter(Collider collider)
        {
            var target = collider.gameObject.GetComponent<SpawnComponent>();
            if (target != null)
            {
                GameContext.LevelController.Current.State.UpdateScore(target.RewardScore);
                //TODO
            }
        }
    }
}
