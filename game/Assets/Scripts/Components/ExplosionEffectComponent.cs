using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Utils;

namespace DevGate
{
    public class ExplosionEffectComponent : MonoBehaviour
    {
        private Signal _onComplete;
        private Lifetime.Definition _definition;

        private void OnEnable()
        {
            _definition = Lifetime.Define(Lifetime.Eternal);
            _onComplete = new Signal(_definition.Lifetime);
        }

        private void OnDisable()
        {
            _definition.Terminate();
        }

        public void SubscribeOnComplete(Lifetime lifetime, Action listener)
        {
            _onComplete.Subscribe(lifetime, listener);
        }

        public void FireOnComplete()
        {
            _onComplete.Fire();
        }

        public void Play()
        {
            GetComponent<ParticleSystem>().Play(true);
        }
    }
}
