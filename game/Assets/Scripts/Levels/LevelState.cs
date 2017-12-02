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
        private bool _isPause = false;

        public int Life { get { return _life; } }

        public int Score { get { return _score; } }

        public bool IsPause { get { return _isPause; } set { _isPause = value; _onStateChanged.Fire(this); } }

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
            if (_life <= 0)
            {
                _life = 0;
                LoosePopup.ShowPopup();
            }
        }

        public void UpdateScore(int delta)
        {
            _score += delta;
            _onStateChanged.Fire(this);
            if (_score >= GameContext.LevelController.Current.Settings.ScoreForFirstLevel)
            {
                WinPopup.ShowPopup();
            }
        }

        public void ResetLevelState()
        {
            _life = 5;
            _score = 0;
            _onStateChanged.Fire(this);
        }
    }
}
