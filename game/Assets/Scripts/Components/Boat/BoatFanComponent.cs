using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DevGate
{
    [ExecuteInEditMode]
    public class BoatFanComponent : MonoBehaviour
    {
        public Transform Transform;
        public float Speed = 1f;

        private void Update()
        {
            Transform.Rotate(new Vector3(0, 0, 1), Speed * Time.deltaTime);
        }
    }
}
