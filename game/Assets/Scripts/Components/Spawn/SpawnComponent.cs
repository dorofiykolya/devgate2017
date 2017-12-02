using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DevGate
{
    public class SpawnComponent : MonoBehaviour
    {
        public float Speed;
        public float Velocity;

        private float _velocity;

        private void Update()
        {
            _velocity += Velocity * Time.deltaTime;
            var speed = Speed + _velocity;

            transform.Translate(new Vector3(0, speed * Time.deltaTime, 0));
        }
    }
}
