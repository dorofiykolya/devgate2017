using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatRotationComponent : MonoBehaviour
{
    private bool resetRotation = false;
    private Vector3 targetRotation;
    private float speed = 6f;

    void Start()
    {
        GameContext.LevelController.Current.InputController.SubscribeOnHorizontalCHange(GameContext.Lifetime, SetHorizontalPosition);
        GameContext.LevelController.Current.InputController.SubscribeOnShoot(GameContext.Lifetime,  ResetRotation);
    }

    private void SetHorizontalPosition(float value)
    {
        Vector3 r = transform.localEulerAngles;
        r.y = value;
        transform.localEulerAngles = r;
    }

    private void ResetRotation()
    {
        targetRotation = transform.localEulerAngles;
        targetRotation.y = 0;
        resetRotation = true;
    }

    private void Update()
    {
        if (!resetRotation) return;
        var r = transform.localEulerAngles;
        r.y = Mathf.LerpAngle(r.y, 0, speed * Time.deltaTime);
        transform.localEulerAngles = r;
        resetRotation = Mathf.Abs(r.y) > 0.02f;
    }
}
