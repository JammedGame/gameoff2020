using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform target;
    public float smoothTime = 0.3F;
    public Vector3 followDistance = new Vector3(0, 1f, -1f);

    private Vector3 velocity = Vector3.zero;

    void Update()
    {
        // Define a target position above and behind the target transform
        Vector3 targetPosition = target.transform.position + followDistance;

        // Smoothly move the camera towards that target position
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }

    void Start() 
    {

    }
}
