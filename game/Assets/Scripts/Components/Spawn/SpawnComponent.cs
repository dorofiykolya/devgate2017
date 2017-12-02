using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Utils;

namespace DevGate
{
    public class SpawnComponent : MonoBehaviour
    {
        public int RewardScore;
        public SpawnType Type;
        public bool Hit;
        public RuneId RuneId;

        public virtual IEnumerator PlayDestroy()
        {
            _onPlayDestroy.Fire();
            yield break;
        }

        public void SubscribeOnPlayDestroyComplete(Lifetime lifetime, Action listener)
        {
            _onPlayDestroy.Subscribe(lifetime, listener);
        }

        protected void FireOnDestroyComplete()
        {
            _onPlayDestroy.Fire();
        }

        private Signal _onPlayDestroy;
        private Lifetime.Definition _definition;

        private void Awake()
        {
            _definition = Lifetime.Define(GameContext.LevelController.Current.Lifetime);
            _onPlayDestroy = new Signal(_definition.Lifetime);
        }

        private void OnDestroy()
        {
            _definition.Terminate();
        }
    }
}
