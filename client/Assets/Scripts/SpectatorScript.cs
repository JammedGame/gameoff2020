using UnityEngine;

public class SpectatorScript : MonoBehaviour
{
    public float maxSpeed;
    public float acceleration;
    public float speedFallOff = 0.5f;
    public float smoothTime = 0.3f;

    private Vector3 currentSpeed = Vector3.zero;
    private Vector3 targetPosition;
    private Vector3 smoothCurrentVelocity = Vector3.zero;
    
    private void Start()
    {
        targetPosition = transform.position;
    }

    private void Update()
    {
        // rotate
        transform.Rotate(-Input.GetAxisRaw("Mouse Y"), Input.GetAxisRaw("Mouse X"), 0);

        // speed fall off
        var speedFallOffThisTick = speedFallOff * Time.deltaTime;
        currentSpeed *= 1 - speedFallOffThisTick;

        // accelerate
        var v = Input.GetAxisRaw("Vertical");
        var h = Input.GetAxisRaw("Horizontal");
        currentSpeed += (h * transform.right + v * transform.forward) * acceleration * Time.deltaTime;

        // clamp speed
        Vector3.ClampMagnitude(currentSpeed, maxSpeed);

        // move
        targetPosition += currentSpeed;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref smoothCurrentVelocity, smoothTime);
    }
}