using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils;

namespace DevGate
{
    public class LevelState
    {
        private int _life;
        private int _score;

        public int Life { get { return _life; } }

        public int Score { get { return _score; } }

        private Signal<LevelState> _onStateChanged;

        public void Init(Lifetime lifetime)
        {
            _onStateChanged = new Signal<LevelState>(lifetime);
            ResetLevelState();
        }

        public void SubscribeOnStateChanged(Lifetime lifetime, Action<LevelState> callback)
        {
            _onStateChanged.Subscribe(lifetime, callback);
        }

        public void UpdateLife(int delta)
        {
            _life += delta;
            _onStateChanged.Fire(this);
        }

        public void UpdateScore(int delta)
        {
            _score += delta;
            _onStateChanged.Fire(this);
        }

        public void ResetLevelState()
        {
            _life = 5;
            _score = 0;
            _onStateChanged.Fire(this);
        }
    }
}
