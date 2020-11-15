using Settings;
using UnityEngine;

namespace Logic
{
    public class Drone
    {
        private Fighter target;
        private float timeSinceLastShot;

        public DroneSettings Settings { get; private set; }
        public DroneType Type => Settings.type;
        public Allegiance Allegiance { get; private set; }
        public float CurrentHealth { get; private set; }
        public Vector3 Position { get; private set; }
        public Quaternion Rotation { get; private set; }
        public Vector3 Velocity { get; private set; }

        public Drone(DroneSettings settings, Allegiance allegiance, Vector3 initialPosition)
            => (Settings, Allegiance, CurrentHealth, Position) = (settings, allegiance, settings.maxHealth, initialPosition);

        public void Tick(float dT)
        {
            timeSinceLastShot += dT;
            if (target == null || target.IsDead) FindTarget();
            if (target == null) return;

            var targetVector = target.Position - Position;
            Rotation = Quaternion.LookRotation(targetVector.normalized);

            if (targetVector.magnitude > Settings.attackRange) MoveTowardsTarget();
            else ShootIfAble();

            Position += Velocity * dT;
        }

        private void FindTarget()
        {
            target = GameController.Instance.GetNearestTarget(Position, Allegiance.GetOpponent());
        }

        private void MoveTowardsTarget()
        {
            var desiredVelocity = Rotation * Vector3.forward * Settings.maxSpeed;
            Velocity = desiredVelocity;
        }

        private void ShootIfAble()
        {
            Velocity = Vector3.zero;
            if (timeSinceLastShot < Settings.attackSpeed) return;

            timeSinceLastShot = 0;
            var projectile = new WeaponProjectile(
                this,
                Settings.attackDamage,
                Position,
                Rotation,
                Rotation * Vector3.forward * Settings.projectileSpeed
            );
            GameController.Instance.AddProjectile(projectile);
        }
    }
}