using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform target;
    public float smoothTime = 0.3F;
    public Vector3 followDistance = new Vector3(0, 1f, -1f);

    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        transform.rotation = target.rotation;

        // Define a target position above and behind the target transform
        var relativeFollow = followDistance.x * target.right + followDistance.y * target.up + followDistance.z * target.forward;
        Vector3 targetPosition = target.transform.position + relativeFollow;

        // Smoothly move the camera towards that target position
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }

    void Start() 
    {

    }
}
