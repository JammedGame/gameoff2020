using Communication;
using Settings;
using UnityEngine;

namespace Logic
{
    public struct FighterInput
    {
        public Vector2 SteerTarget;
        public float Throttle;
        public float Roll;
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
            var state = new PlayerState();
            state.CopyFrom(previousState);

            // rotate
            var newRotation = state.rotation * Quaternion.Euler(
                -input.SteerTarget.y * settings.steerSpeed * dT,
                input.SteerTarget.x * settings.steerSpeed * dT,
                -input.Roll * settings.rollSpeed * dT
            );
            state.rotation = newRotation;

            // velocity
            var targetSpeed = input.Throttle >= 0
                ? Mathf.Lerp(settings.defaultSpeed, settings.boostSpeed, input.Throttle)
                : Mathf.Lerp(settings.defaultSpeed, settings.brakeSpeed, -input.Throttle);
            var targetVelocity = newRotation * Vector3.forward * targetSpeed;
            var newVelocity = Vector3.Lerp(state.velocity, targetVelocity, Mathf.Exp(-settings.velocitySmooth / dT));
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
