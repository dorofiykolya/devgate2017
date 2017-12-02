using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatRotationComponent : MonoBehaviour
{
    private float speed = 1f;
    private Vector3 targetRotation = Vector3.zero;

    void Start()
    {
        GameContext.LevelController.Current.InputController.SubscribeOnTouchProcess(GameContext.Lifetime, SetTargetAngle);
    }

    private void SetTargetAngle(TouchPhase phase, float deltaX)
    {
        if (phase == TouchPhase.Moved && deltaX != 0)
        {

        }
        else
            targetRotation = Vector3.zero;
    }

    private void Update()
    {
//         if(Input)
//         var r = transform.localEulerAngles;
//         r.y = Mathf.LerpAngle(r.y, 0, speed * Time.deltaTime);
//         transform.localEulerAngles = r;
//         resetRotation = Mathf.Abs(r.y) > 0.02f;
    }
}
