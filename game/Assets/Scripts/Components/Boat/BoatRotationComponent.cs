using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatRotationComponent : MonoBehaviour
{

    void Start()
    {
        GameContext.LevelController.Current.InputController.SubscribeOnHorizontalCHange(GameContext.Lifetime, SetHorizontalPosition);
    }

    private void SetHorizontalPosition(float value)
    {
        Vector3 r = transform.localEulerAngles;
        r.y = value;
        transform.localEulerAngles = r;
    }
}
