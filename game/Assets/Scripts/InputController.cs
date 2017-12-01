using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class InputController : MonoBehaviour
{

    private float _currentPower = 0;
    private float _horizontalPosition = 0;

    public float CurrentPower { get { return _currentPower; } }
    public float HorizontalPosition { get { return _horizontalPosition; } }

    private Vector2 _startTouchPosition = Vector2.zero;

    private Signal _onShoot;

    private void Awake()
    {
        Input.simulateMouseWithTouches = true;
    }

    void Update()
    {
        Debug.Log(Input.touchCount);
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

    public void SubscribeOnShoot(Lifetime lifetime,Action callback)
    {
        _onShoot.Subscribe(lifetime, callback);
    }
}
