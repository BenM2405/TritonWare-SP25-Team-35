using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector2 offset = new Vector2(0f, 1.5f);
    public float smoothTime = 0.08f;

    Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desired = new Vector3(target.position.x + offset.x, 
        target.position.y + offset.y, 
        transform.position.z);

        transform.position = Vector3.SmoothDamp(transform.position, desired, ref velocity, smoothTime);
    }
}
