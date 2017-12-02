using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatRotationComponent : MonoBehaviour
{
    private float speed = 4f;
    private Vector3 targetRotation = Vector3.zero;

    void Start()
    {
        GameContext.LevelController.Current.InputController.SubscribeOnTouchProcess(GameContext.Lifetime, SetTargetAngle);
    }

    private void SetTargetAngle(TouchPhase phase, float deltaX)
    {
        if (phase == TouchPhase.Moved && deltaX != 0)
        {
            targetRotation = deltaX > 0 ? new Vector3(0, 0, 20) : new Vector3(0, 0, -20);
        }
        else
            targetRotation = Vector3.zero;
    }

    private void Update()
    {
        var r = transform.localEulerAngles;
        r.z = Mathf.LerpAngle(r.z, targetRotation.z, (targetRotation.z == 0 ? 2 : 1f) * speed * Time.deltaTime);
        transform.localEulerAngles = r;
    }
}
