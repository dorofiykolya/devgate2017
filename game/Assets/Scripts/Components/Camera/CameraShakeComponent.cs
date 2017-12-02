using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DevGate
{
    public class CameraShakeComponent : MonoBehaviour
    {
        public float ShakeTime;
        public Vector3 ShakeOffset;

        private float _remainingTime;
        private Coroutine _coroutine;

        public void Shake()
        {
            _remainingTime = ShakeTime;
            if (_coroutine == null)
            {
                _coroutine = StartCoroutine(ShakeInternal());
            }
        }

        private IEnumerator ShakeInternal()
        {
            while (true)
            {
                _remainingTime -= Time.deltaTime;
                if (_remainingTime <= 0f) break;

                var x = UnityEngine.Random.Range(0, ShakeOffset.x);
                var y = UnityEngine.Random.Range(0, ShakeOffset.y);
                var z = UnityEngine.Random.Range(0, ShakeOffset.z);

                transform.localPosition = new Vector3(x, y, z);

                yield return null;
            }
            transform.localPosition = Vector3.zero;
            _coroutine = null;
        }
    }
}
