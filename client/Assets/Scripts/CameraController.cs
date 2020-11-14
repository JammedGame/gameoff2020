using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public Vector3 followDistance = new Vector3(0, 1f, -1f);
    public Vector3 smooth;
    public AnimationCurve FollowZ;

    Vector3 lastPosition;
    float currentZFollow;
    bool firstUpdate;

    void LateUpdate()
    {
        if (target == null)
            return;

        if (!firstUpdate)
            lastPosition = transform.position;

        // rotate
        transform.rotation = target.rotation;

        // Define a target position above and behind the target transform
        var velocityZ = Vector3.Dot(target.position - lastPosition, target.forward) / Time.deltaTime;
        var targetZFollow = FollowZ.Evaluate(velocityZ);

        currentZFollow = !firstUpdate ? targetZFollow : LerpCombine(currentZFollow, targetZFollow, 0.1f, 0.5f);

        var relativeFollow = followDistance.x * target.right + followDistance.y * target.up + currentZFollow * target.forward;
        var targetPosition = target.position + relativeFollow;

        if (!firstUpdate)
        {
            transform.position = targetPosition;
            velocityZ = 0;
            firstUpdate = true;
        }

        var newCameraPos = transform.position;
        var dir = targetPosition - newCameraPos;
        var dirAlongX = Vector3.Project(dir, target.right);
        var dirAlongY = Vector3.Project(dir, target.up);
        var dirAlongZ = Vector3.Project(dir, target.forward);
        newCameraPos += dirAlongX * smooth.x;
        newCameraPos += dirAlongY * smooth.y;
        newCameraPos += dirAlongZ * smooth.z;
        transform.position = newCameraPos;

        lastPosition = target.position;
    }

    public float LerpCombine(float a, float b, float constantDeltaPerSecond, float linearDeltaPerSecond)
    {
        var dT = Time.deltaTime;
        var newValue = Mathf.Lerp(a, b, linearDeltaPerSecond * dT);
        newValue = Mathf.MoveTowards(newValue, b, constantDeltaPerSecond * dT);
        return newValue;
    }

}