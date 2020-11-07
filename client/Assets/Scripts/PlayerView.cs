using UnityEngine;

public class PlayerView : MonoBehaviour
{
    public float maxSpeed;
    public float acceleration;
    public float speedFallOff = 0.5f;

    private Vector3 currentVelocity = Vector3.zero;

    private void Update()
    {
        // rotate
        transform.Rotate(-Input.GetAxisRaw("Mouse Y"), Input.GetAxisRaw("Mouse X"), 0);

        // speed fall off
        currentVelocity = Vector3.Lerp(currentVelocity, Vector3.zero, speedFallOff * Time.deltaTime);

        // accelerate
        var v = Input.GetAxisRaw("Vertical");
        var h = Input.GetAxisRaw("Horizontal");
        currentVelocity += (h * transform.right + v * transform.forward) * acceleration * Time.deltaTime;

        // clamp speed
        Vector3.ClampMagnitude(currentVelocity, maxSpeed);

        // move
        transform.position += currentVelocity;
    }
}