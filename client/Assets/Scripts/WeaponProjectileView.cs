using UnityEngine;

public class WeaponProjectileView : MonoBehaviour 
{
    private Vector3 velocity = Vector3.zero;

    private void Update()
    {
        // move
        var newPosition = transform.position + velocity * Time.smoothDeltaTime;
        transform.position = newPosition;
    }

    public void SetPositionAndVelocity(Transform owner, Vector3 velocity) 
    {
        transform.SetPositionAndRotation(owner.position, owner.rotation);
        this.velocity = velocity;

    }
}