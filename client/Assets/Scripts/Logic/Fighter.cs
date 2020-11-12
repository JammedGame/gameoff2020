using Communication;
using Settings;
using UnityEngine;

namespace Logic
{
    public struct FighterInput
    {
        public Vector2 CrosshairPosition;
        public Vector3 Acceleration;
        public bool Shoot;
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

        public void SetPlayerInput(FighterInput command) => currentInput = command;

        public void Tick(float dT)
        {
            state = Simulate(state, currentInput, dT);
        }

        public PlayerState Simulate(PlayerState previousState, FighterInput input, float dT)
        {
            // rotate
            var state = previousState;
            state.rotation *= Quaternion.Euler(-input.CrosshairPosition.y * settings.steeringSpeed, input.CrosshairPosition.x * settings.steeringSpeed, 0);

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
            if (input.Shoot && timeSinceLastShot >= settings.attackSpeed) Shoot();
            return state;
        }

        private void Shoot()
        {
            timeSinceLastShot = 0;

            var projectile = new WeaponProjectile(
                state.position,
                state.rotation,
                state.velocity + state.rotation * Vector3.forward * settings.projectileSpeed
            );
            GameController.Instance.AddProjectile(projectile);
        }
    }
}
