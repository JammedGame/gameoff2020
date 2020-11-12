using Communication;
using Settings;
using UnityEngine;

namespace Logic
{
    public struct FighterInput
    {
        public Vector2 SteerTarget;
        public Vector3 Acceleration;
        public Quaternion ShootDirection;
        public bool Shoot;
        public bool Drift;
    }

    public class Fighter
    {
        private FighterSettings settings;
        private PlayerState state;
        private float timeSinceLastShot;
        private FighterInput currentInput;

        public PlayerState State => state;
        public string PlayerId => state.id;
        public Vector3 Position => state.position;
        public Quaternion Rotation => state.rotation;

        public Fighter(FighterSettings settings, PlayerState state) => (this.settings, this.state) = (settings, state);

        public void SetPlayerInput(FighterInput input) => currentInput = input;

        public void Tick(float dT)
        {
            state = Simulate(state, currentInput, dT);
        }

        public PlayerState Simulate(PlayerState previousState, FighterInput input, float dT)
        {
            // rotate
            var state = previousState;
            state.rotation *= Quaternion.Euler(-input.SteerTarget.y * settings.steeringSpeed, input.SteerTarget.x * settings.steeringSpeed, 0);

            // speed fall off
            var newVelocity = Vector3.Lerp(state.velocity, Vector3.zero, settings.speedFallOff * dT);

            // accelerate
            var controllerAcceleration = state.rotation * input.Acceleration * settings.acceleration;
            var gravityAcceleration = GravityController.Instance.getGravityAcceleration(state.position);
            newVelocity += (controllerAcceleration + gravityAcceleration) * dT;

            // clamp speed
            newVelocity = Vector3.ClampMagnitude(newVelocity, settings.maxSpeed);
            state.velocity = newVelocity;

            // move
            state.position += newVelocity * dT;

            // shoot
            timeSinceLastShot += dT;
            if (input.Shoot && timeSinceLastShot >= settings.attackSpeed) Shoot(input.ShootDirection);
            return state;
        }

        private void Shoot(Quaternion shootDirection)
        {
            timeSinceLastShot = 0;

            var projectileRotation = state.rotation * shootDirection;
            var projectile = new WeaponProjectile(
                state.position,
                projectileRotation,
                state.velocity + projectileRotation * Vector3.forward * settings.projectileSpeed
            );
            GameController.Instance.AddProjectile(projectile);
        }
    }
}
