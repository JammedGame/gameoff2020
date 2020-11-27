using Communication;
using Settings;
using UnityEngine;

namespace Logic
{
    public class Drone : BattleObject
    {
        private Fighter target;
        private float timeSinceLastShot;

        public DroneSettings Settings { get; private set; }
        public DroneType Type => Settings.type;
        public Quaternion Rotation { get; private set; }
        public Vector3 Velocity { get; private set; }

        public Drone(DroneSettings settings, Team team, Vector3 initialPosition, Quaternion initialRotation)
            => (Settings, Team, CurrentHealth, Position, Rotation, CollisionScale) = (settings, team, settings.maxHealth, initialPosition, initialRotation, settings.collisionScale);

        public void Tick(float dT)
        {
            timeSinceLastShot += dT;
            if (target == null || target.Dead) FindTarget();
            if (target == null) return;

            var targetVector = target.Position - Position;
            Rotation = Quaternion.LookRotation(targetVector.normalized);

            if (targetVector.magnitude > Settings.attackRange) MoveTowardsTarget();
            else ShootIfAble();

            Position += Velocity * dT;
        }

        private void FindTarget()
        {
            target = GameController.Instance.GetNearestTarget(Position, Team.GetOpponent());
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