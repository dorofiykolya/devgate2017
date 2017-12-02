using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    private bool resetRotation = false;
    private Vector3 targetRotation;
    private float speed = 2f;
    private float minX = -50;
    private float maxX = 50;

    void Start ()
    {
        GameContext.LevelController.Current.InputController.SubscribeOnHorizontalCHange(GameContext.Lifetime, MoveCamera);
    }

	void MoveCamera(float value)
    {
        Vector3 r = transform.localPosition;
        r.x = Mathf.Clamp(r.x + value * 0.6f, minX, maxX);
        transform.localPosition = r;
    }
}
