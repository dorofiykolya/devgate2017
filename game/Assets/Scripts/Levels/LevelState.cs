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

        private bool _gameOver = false;

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
            if (_gameOver) return;
            _life += delta;
            _onStateChanged.Fire(this);
            if (_life <= 0)
            {
                _life = 0;
                _gameOver = true;
                LoosePopup.ShowPopup();
            }
        }

        public void UpdateScore(int delta)
        {
            if (_gameOver) return;
            _score += delta;
            _onStateChanged.Fire(this);
            if (_score >= GameContext.LevelController.Current.Settings.ScoreForFirstLevel)
            {
                _gameOver = true;
                WinPopup.ShowPopup();
            }
        }

        public void ResetLevelState()
        {
            _gameOver = false;
            _life = 5;
            _score = 0;
            _onStateChanged.Fire(this);
        }
    }
}
