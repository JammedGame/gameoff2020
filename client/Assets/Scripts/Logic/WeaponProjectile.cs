using UnityEngine;

namespace Logic
{
    public class WeaponProjectile
    {
        private Vector3 position;
        private Quaternion rotation;
        private Vector3 velocity;

        public Vector3 Position => position;
        public Quaternion Rotation => rotation;

        public WeaponProjectile(Vector3 position, Quaternion rotation, Vector3 velocity) =>
            (this.position, this.rotation, this.velocity) = (position, rotation, velocity);

        public void Tick(float dT)
        {
            position += velocity * dT;
        }
    }
}