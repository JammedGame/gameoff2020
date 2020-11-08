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
        transform.position = State.position;
    }

    private void Update()
    {
        // rotate
        transform.Rotate(-Input.GetAxisRaw("Mouse Y"), Input.GetAxisRaw("Mouse X"), 0);

        // speed fall off
        var newVelocity = Vector3.Lerp(State.velocity, Vector3.zero, speedFallOff * Time.deltaTime);

        // accelerate
        var v = Input.GetAxisRaw("Vertical");
        var h = Input.GetAxisRaw("Horizontal");
        var controllerAcceleration = (h * transform.right + v * transform.forward) * acceleration;
        var gravityAcceleration = GravityController.Instance.getGravityAcceleration(State.position);
        newVelocity += (controllerAcceleration + gravityAcceleration) * Time.deltaTime;

        // clamp speed
        newVelocity = Vector3.ClampMagnitude(newVelocity, maxSpeed);
        State.velocity = newVelocity;

        // move
        var newPosition = State.position + newVelocity * Time.smoothDeltaTime;
        State.position = newPosition;
        transform.position = newPosition;
    }
}