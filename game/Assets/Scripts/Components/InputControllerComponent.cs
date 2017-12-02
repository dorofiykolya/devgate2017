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
        private float _horizontalDelta = 0;

        public float CurrentPower
        {
            get { return _currentPower; }
        }

        public float HorizontalDelta
        {
            get { return _horizontalDelta; }
        }

        private Vector2 _startTouchPosition = Vector2.zero;

        private Signal _onShoot;
        private Signal<float> _onHorizontalChange;
        private Signal<TouchPhase, float> _onTouchProcess;

        private bool _mousePressed = false;
        private LevelSettingsScriptableObject settings;

        private float touchTime;
        private float clickTime = 0.5f;

        public void Init(Lifetime lifetime)
        {
            
            Game.Instance.OnUpdate.Subscribe(lifetime, OnUpdate);
            _onShoot = new Signal(lifetime);
            _onHorizontalChange = new Signal<float>(lifetime);
            _onTouchProcess = new Signal<TouchPhase, float>(lifetime);
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
                    _onTouchProcess.Fire(TouchPhase.Began, 0);
                    _startTouchPosition = touch.position;
                    touchTime = Time.time;
                }
                else if (touch.phase == TouchPhase.Moved)
                {
                    settings = GameContext.LevelController.Current.Settings;
                    _currentPower = _startTouchPosition.y - touch.position.y;
                    _horizontalDelta = touch.deltaPosition.x / Screen.dpi * settings.ScreenDragCoeff;
                    _onTouchProcess.Fire(TouchPhase.Moved, _horizontalDelta);
                    _onHorizontalChange.Fire(_horizontalDelta);
                }
                else if (touch.phase == TouchPhase.Ended)
                {
                    if (Time.time - touchTime <= clickTime)
                    {
                        _onShoot.Fire();
                    }
                    _currentPower = 0;
                    _horizontalDelta = 0;
                    _startTouchPosition = Vector2.zero;
                    _onTouchProcess.Fire(TouchPhase.Ended, 0);
                }
            }
        }

        private void ProcessMouse()
        {
            if (Input.GetMouseButtonUp(0))
            {
                if (Time.time - touchTime <= clickTime)
                {
                    _onShoot.Fire();
                }
                _currentPower = 0;
                _horizontalDelta = 0;
                _startTouchPosition = Vector2.zero;
                _mousePressed = false;
                _onTouchProcess.Fire(TouchPhase.Ended, 0);
                return;

            }
            if (Input.GetMouseButtonDown(0) && !_mousePressed)
            {
                _startTouchPosition = Input.mousePosition;
                _mousePressed = true;
                touchTime = Time.time;
                _onTouchProcess.Fire(TouchPhase.Began, 0);
            }
            else if (Input.GetMouseButton(0) && _mousePressed)
            {
                settings = GameContext.LevelController.Current.Settings;
                _currentPower = _startTouchPosition.y - Input.mousePosition.y;
                _horizontalDelta = (Input.mousePosition.x - _startTouchPosition.x) / Screen.dpi * settings.ScreenDragCoeff;
                _startTouchPosition = Input.mousePosition;
                _onTouchProcess.Fire(TouchPhase.Moved, _horizontalDelta);
                _onHorizontalChange.Fire(_horizontalDelta);
            }
        }

        public void SubscribeOnShoot(Lifetime lifetime, Action callback)
        {
            _onShoot.Subscribe(lifetime, callback);
        }

        public void SubscribeOnHorizontalCHange(Lifetime lifetime, Action<float> callback)
        {
            _onHorizontalChange.Subscribe(lifetime, callback);
        }

        public void SubscribeOnTouchProcess(Lifetime lifetime, Action<TouchPhase, float> callback)
        {
            _onTouchProcess.Subscribe(lifetime, callback);
        }
    }
}