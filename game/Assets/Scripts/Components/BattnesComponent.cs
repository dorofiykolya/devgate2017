using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DevGate
{
    public class BattnesComponent : MonoBehaviour
    {
        public Transform[] Transforms;
        private float _offset = 5.5f;
        private Vector3[] _init;
        private float _speed;

        private void Awake()
        {
            _init = new Vector3[Transforms.Length];
            for (var i = 0; i < Transforms.Length; i++)
            {
                var t = Transforms[i];
                _init[i] = t.localPosition;
            }
        }

        private Transform _current;
        private Vector3 _initVector3;
        private float _ratio = 1f;

        private IEnumerator Start()
        {
            while (true)
            {
                yield return null;
                if (_ratio >= 1)
                {
                    yield return new WaitForSeconds(.5f);
                    _ratio = 0;
                    var index = UnityEngine.Random.Range(0, Transforms.Length);
                    _speed = UnityEngine.Random.Range(0.5f, 5f);
                    _current = Transforms[index];
                    _initVector3 = _init[index];
                }
                _ratio += (_speed * Time.deltaTime);
                var pos = _current.transform.localPosition;
                pos.y = _initVector3.y + _offset * Mathf.Sin(_ratio * Mathf.PI);
                _current.transform.localPosition = pos;
            }
        }
    }
}
