using UnityEngine;
using Communication;

public class PlayerView : MonoBehaviour
{
    public float maxSpeed;
    public float acceleration;
    public float speedFallOff = 0.5f;

    public PlayerState State { get; set; }

    private void Start()
    {
        transform.position = State.Position;
    }

    private void Update()
    {
        // rotate
        transform.Rotate(-Input.GetAxisRaw("Mouse Y"), Input.GetAxisRaw("Mouse X"), 0);

        // speed fall off
        var newVelocity = Vector3.Lerp(State.Velocity, Vector3.zero, speedFallOff * Time.deltaTime);

        // accelerate
        var v = Input.GetAxisRaw("Vertical");
        var h = Input.GetAxisRaw("Horizontal");
        newVelocity += (h * transform.right + v * transform.forward) * acceleration * Time.deltaTime;

        // clamp speed
        newVelocity = Vector3.ClampMagnitude(newVelocity, maxSpeed);
        State.Velocity = newVelocity;

        // move
        var newPosition = State.Position + newVelocity * Time.deltaTime;
        transform.position = State.Position = newPosition;
    }
}