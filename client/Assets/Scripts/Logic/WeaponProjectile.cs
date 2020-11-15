using Communication;
using UnityEngine;

namespace Logic
{
    public class WeaponProjectile
    {
        private object owner;
        private float damage;
        private ProjectileState state;
        public Vector3 Position => state.position;
        public Quaternion Rotation => state.rotation;
        public float Time => state.time;

        public WeaponProjectile(object owner, float damage, Vector3 position, Quaternion rotation, Vector3 velocity) =>
            (this.owner, this.damage, state) = (owner, damage, new ProjectileState
            {
                position = position,
                rotation = rotation,
                velocity = velocity,
            });

        public void Tick(float dT)
        {
            state.position += state.velocity * dT;
            state.time += dT;
            state.velocity *= 1.01f; // todo: add some kind of initial-max speed thing?
        }
    }
}