using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DevGate
{
    public class BoatRocketComponent : MonoBehaviour
    {

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
