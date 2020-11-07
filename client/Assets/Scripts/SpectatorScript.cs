using UnityEngine;

public class SpectatorScript : MonoBehaviour
{
    public float maxSpeed;
    public float acceleration;
    public float speedFallOff = 0.5f;

    private Vector3 currentSpeed = Vector3.zero;
    
    private void FixedUpdate()
    {
        // rotate
        transform.Rotate(Input.GetAxisRaw("Mouse Y"), Input.GetAxisRaw("Mouse X"), 0);

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
        transform.position += currentSpeed;
        
    }
}