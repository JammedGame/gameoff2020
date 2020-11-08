using UnityEngine;
using Communication;

public class PlayerView : MonoBehaviour
{
    public float maxSpeed;
    public float acceleration;
    public float speedFallOff = 0.5f;

    public PlayerState State;

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
        var controllerAcceleration = (h * transform.right + v * transform.forward) * acceleration;
        var gravityAcceleration = GravityController.Instance.getGravityAcceleration(State.Position);
        newVelocity += (controllerAcceleration + gravityAcceleration) * Time.deltaTime;

        // clamp speed
        newVelocity = Vector3.ClampMagnitude(newVelocity, maxSpeed);
        State.Velocity = newVelocity;

        // move
        var newPosition = State.Position + newVelocity * Time.smoothDeltaTime;
        State.Position = newPosition;
        transform.position = newPosition;
    }
}