using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DevGate
{
    public class RuneSwapComponent : SpawnComponent
    {
        public MeshRenderer MeshRenderer;
        public float DissolveTime = 1f;

        private Material _material;
        private float _remainingTime;

        private void OnEnable()
        {
            if (_material != null)
            {
                _material.SetFloat("_Dissolve", 0f);
            }
        }

        public override IEnumerator PlayDestroy()
        {
            yield return null;

            _remainingTime = DissolveTime;
            _material = MeshRenderer.material;
            
            while (_remainingTime >= 0)
            {
                _remainingTime -= Time.deltaTime;
                _material.SetFloat("_Dissolve", (DissolveTime - _remainingTime) / DissolveTime);
                yield return null;
            }

            FireOnDestroyComplete();
        }
    }
}
