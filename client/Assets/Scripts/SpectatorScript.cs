using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpectatorScript : MonoBehaviour
{

    public float maxSpeed;

    public float acceleration;

    public float speedFallOff = 0.5f;
    private Vector3 currentSpeed = Vector3.zero;
    
    private void FixedUpdate()
    {
        float v = Input.GetAxisRaw("Vertical");
        float h = Input.GetAxisRaw("Horizontal");

        // Vector2 rotation = new Vector2 (0, 0);

        // rotation.y += Input.GetAxis ("Mouse X");
        // rotation.x += -Input.GetAxis ("Mouse Y");
        // transform.eulerAngles = (Vector2)rotation * currentSpeed;
        transform.Rotate(Input.GetAxisRaw("Mouse Y"), Input.GetAxisRaw("Mouse X"), 0);

        // speed fall off
        var speedFallOffThisTick = speedFallOff * Time.deltaTime;
        currentSpeed *= 1 - speedFallOffThisTick;

        // accelerate
        currentSpeed += (h * transform.right + v * transform.forward) * acceleration * Time.deltaTime;

        // clamp speed
        Vector3.ClampMagnitude(currentSpeed, maxSpeed);

        // currentSpeed = new Vector3((currentSpeed.x + acceleration * h currentSpeed.x) % maxSpeed, 0, (currentSpeed.z * 0.2F * currentSpeed.z * v) % maxSpeed);
        // Debug.Log(h);
        transform.position += currentSpeed;
        
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
