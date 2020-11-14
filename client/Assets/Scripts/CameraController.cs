using UnityEngine;
using View;

public class CameraController : MonoBehaviour
{
    public Vector3 followDistance = new Vector3(0, 1f, -1f);
    public Vector3 smooth;
    public AnimationCurve FollowZ;
    public AnimationCurve SpeedFovEffect;

    Camera cam;
    Vector3 lastPosition;
    float currentZFollow;
    bool firstUpdate;

    public FighterView target { get; set; }

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        if (target == null)
            return;

        if (!firstUpdate)
            lastPosition = transform.position;

        // rotate
        var targetTransform = target.transform;
        transform.rotation = targetTransform.rotation;

        // Define a target position above and behind the target transform
        var velocityZ = Vector3.Dot(targetTransform.position - lastPosition, targetTransform.forward) / Time.deltaTime;
        var targetZFollow = FollowZ.Evaluate(velocityZ);

        currentZFollow = !firstUpdate ? targetZFollow : LerpCombine(currentZFollow, targetZFollow, 0.1f, 0.5f);

        var relativeFollow = followDistance.x * targetTransform.right + followDistance.y * targetTransform.up + currentZFollow * targetTransform.forward;
        var targetPosition = targetTransform.position + relativeFollow;

        if (!firstUpdate)
        {
            transform.position = targetPosition;
            velocityZ = 0;
            firstUpdate = true;
        }

        var newCameraPos = transform.position;
        var dir = targetPosition - newCameraPos;
        var dirAlongX = Vector3.Project(dir, targetTransform.right);
        var dirAlongY = Vector3.Project(dir, targetTransform.up);
        var dirAlongZ = Vector3.Project(dir, targetTransform.forward);
        newCameraPos += dirAlongX * smooth.x;
        newCameraPos += dirAlongY * smooth.y;
        newCameraPos += dirAlongZ * smooth.z;
        transform.position = newCameraPos;

        cam.fieldOfView = SpeedFovEffect.Evaluate(target.Fighter.Velocity.magnitude);

        lastPosition = targetTransform.position;
    }

    public float LerpCombine(float a, float b, float constantDeltaPerSecond, float linearDeltaPerSecond)
    {
        var dT = Time.deltaTime;
        var newValue = Mathf.Lerp(a, b, linearDeltaPerSecond * dT);
        newValue = Mathf.MoveTowards(newValue, b, constantDeltaPerSecond * dT);
        return newValue;
    }

}