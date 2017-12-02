using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	
	void Start ()
    {
        GameContext.LevelController.Current.InputController.SubscribeOnHorizontalCHange(GameContext.Lifetime, RotateCamera);
	}
	

	void RotateCamera(float value)
    {
        Vector3 r = transform.localEulerAngles;
        r.y = value * 0.2f;
        transform.localEulerAngles = r;
    }
}
