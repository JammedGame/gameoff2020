﻿using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public float smoothMovement = 0.8f;
    public Vector3 followDistance = new Vector3(0, 1f, -1f);

    void LateUpdate()
    {
        if (target == null) return;

        // rotate
        transform.rotation = target.rotation;

        // Define a target position above and behind the target transform
        var relativeFollow = followDistance.x * target.right + followDistance.y * target.up + followDistance.z * target.forward;
        var targetPosition = target.transform.position + relativeFollow;

        // Smoothly move the camera towards that target position
        transform.position = Vector3.Lerp(transform.position, targetPosition, 1 - smoothMovement);
    }
}