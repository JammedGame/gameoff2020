using Communication;
using Settings;
using UnityEngine;

namespace Logic
{
    public class WeaponProjectile
    {
        private WeaponProjectileSettings settings;
        private BattleObject owner;
        private ProjectileState state;
        public float Damage { get; private set; }
        public Team Team => owner.Team;
        public Vector3 Position => state.position;
        public Quaternion Rotation => state.rotation;
        public float CollisionScale => settings.collisionScale;
        public BattleObject Owner => owner;
        public float Time => state.time;
        public bool Dead { get; private set; }

        public WeaponProjectile(BattleObject owner, float damage, Vector3 position, Quaternion rotation, Vector3 velocity) =>
            (this.owner, this.Damage, settings, state) = (owner, damage, GameSettings.Instance.WeaponProjectileSettings, new ProjectileState
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

        public void GetKilled(object source)
        {
            Dead = true;
            Debug.Log($"{this} was killed by {source}");
        }
    }
}