using UnityEngine;

namespace Logic
{
    public class WeaponProjectile
    {
        private Vector3 position;
        private Quaternion rotation;
        private float speed;

        public Vector3 Position => position;
        public Quaternion Rotation => rotation;

        public WeaponProjectile(Vector3 position, Quaternion rotation, float speed) =>
            (this.position, this.rotation, this.speed) = (position, rotation, speed);

        public void Tick(float dT)
        {
            position += rotation * Vector3.forward * speed * dT;
        }
    }
}