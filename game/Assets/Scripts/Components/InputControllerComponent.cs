using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace DevGate
{
    public class InputControllerComponent
    {

        private float _currentPower = 0;
        private float _horizontalPosition = 0;

        public float CurrentPower
        {
            get { return _currentPower; }
        }

        public float HorizontalPosition
        {
            get { return _horizontalPosition; }
        }

        private Vector2 _startTouchPosition = Vector2.zero;

        private Signal _onShoot;
        private Signal<float> _onPowerChange;
        private Signal<float> _onHorizontalChange;

        private bool _mousePressed = false;
        private LevelSettingsScriptableObject settings;

        public void Init(Lifetime lifetime)
        {
            
            Game.Instance.OnUpdate.Subscribe(lifetime, OnUpdate);
            _onShoot = new Signal(lifetime);
            _onPowerChange = new Signal<float>(lifetime);
            _onHorizontalChange = new Signal<float>(lifetime);
        }

        void OnUpdate()
        {
#if UNITY_EDITOR
            ProcessMouse();
#else
            ProcessTouches();
#endif
        }

        private void ProcessTouches()
        {
            if (Input.touchCount == 1)
            {
                Touch touch = Input.touches[0];
                if (touch.phase == TouchPhase.Began)
                {
                    _startTouchPosition = touch.position;
                }
                else if (touch.phase == TouchPhase.Moved)
                {
                    _currentPower = _startTouchPosition.y - touch.position.y;
                    _horizontalPosition = touch.position.x - _startTouchPosition.x;
                    _onPowerChange.Fire(_currentPower);
                    _onHorizontalChange.Fire(_horizontalPosition);
                }
                else if (touch.phase == TouchPhase.Ended)
                {
                    _onShoot.Fire();
                    _currentPower = 0;
                    _horizontalPosition = 0;
                    _startTouchPosition = Vector2.zero;
                }
            }
        }

        private void ProcessMouse()
        {
            if (Input.GetMouseButtonUp(0))
            {
                _onShoot.Fire();
                _currentPower = 0;
                _horizontalPosition = 0;
                _startTouchPosition = Vector2.zero;
                _mousePressed = false;
                return;
            }
            if (Input.GetMouseButtonDown(0) && !_mousePressed)
            {
                _startTouchPosition = Input.mousePosition;
                _mousePressed = true;
            }
            else if (Input.GetMouseButton(0) && _mousePressed)
            {
                settings = GameContext.LevelController.Current.Settings;
                _currentPower = _startTouchPosition.y - Input.mousePosition.y;
                var horizontal = (Input.mousePosition.x - _startTouchPosition.x) / Screen.dpi * settings.ScreenDragCoeff;
                var halfAngle = settings.MaxHorizontalAngle * 0.5f;
                _horizontalPosition = Mathf.Clamp(horizontal, -halfAngle, halfAngle);
                _onPowerChange.Fire(_currentPower);
                _onHorizontalChange.Fire(_horizontalPosition);
            }
        }

        public void SubscribeOnShoot(Lifetime lifetime, Action callback)
        {
            _onShoot.Subscribe(lifetime, callback);
        }

        public void SubscribeOnPowerChange(Lifetime lifetime, Action<float> callback)
        {
            _onPowerChange.Subscribe(lifetime, callback);
        }

        public void SubscribeOnHorizontalCHange(Lifetime lifetime, Action<float> callback)
        {
            _onHorizontalChange.Subscribe(lifetime, callback);
        }
    }
}