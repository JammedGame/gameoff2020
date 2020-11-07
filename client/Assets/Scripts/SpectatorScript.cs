using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpectatorScript : MonoBehaviour
{

    public float maxSpeed;

    public float acceleration;

    public float speedFallOff = 10;
    private Vector3 currentSpeed = Vector3.zero;
    public float smoothTime = 0.3F;
    
    private void FixedUpdate()
    {
        float v = Input.GetAxisRaw("Vertical");
        float h = Input.GetAxisRaw("Horizontal");

        // Vector2 rotation = new Vector2 (0, 0);

        // rotation.y += Input.GetAxis ("Mouse X");
        // rotation.x += -Input.GetAxis ("Mouse Y");
        // transform.eulerAngles = (Vector2)rotation * currentSpeed;

        Vector3 newSpeed = new Vector3(
            Mathf.Clamp(currentSpeed.x + (acceleration * h * Time.deltaTime), -maxSpeed, maxSpeed), 
            0, 
            Mathf.Clamp(currentSpeed.z + acceleration * v * Time.deltaTime,-maxSpeed, maxSpeed)
        );

        if(v == 0) {
            if(newSpeed.z > 0) {
                newSpeed.z -= speedFallOff * Time.deltaTime;
            } 
            else if (newSpeed.z < 0) {
                newSpeed.z += speedFallOff * Time.deltaTime; 
            }
        }
        if(h == 0) {
            if(newSpeed.x > 0) {
                newSpeed.x -= speedFallOff * Time.deltaTime;
            } 
            else if (newSpeed.x < 0) {
                newSpeed.x += speedFallOff * Time.deltaTime; 
            }
        }
        currentSpeed = newSpeed;

        // currentSpeed = new Vector3((currentSpeed.x + acceleration * h currentSpeed.x) % maxSpeed, 0, (currentSpeed.z * 0.2F * currentSpeed.z * v) % maxSpeed);
        // Debug.Log(h);
        Vector3 targetPosition = transform.position + new Vector3(h, 0, v);
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
