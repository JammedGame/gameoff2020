using Communication;
using Settings;
using UnityEngine;

namespace Logic
{
    public class Fighter
    {
        private FighterSettings settings;
        private PlayerState state;
        private float timeSinceLastShot;
        private Quaternion inputRotation;
        private Vector3 inputAcceleration;
        private bool inputShoot;

        public PlayerState State => state;
        public string PlayerId => state.id;
        public Vector3 Position => state.position;
        public Quaternion Rotation => state.rotation;

        public Fighter(FighterSettings settings, PlayerState state) => (this.settings, this.state) = (settings, state);

        public void SetPlayerInput(Quaternion inputRotation, Vector3 inputAcceleration, bool inputShoot)
            => (this.inputRotation, this.inputAcceleration, this.inputShoot) = (inputRotation, inputAcceleration, inputShoot);

        public void ClearPlayerInput()
            => (inputRotation, inputAcceleration, inputShoot) = (Quaternion.identity, Vector3.zero, false);

        public void Tick(float dT)
        {
            // rotate
            state.rotation *= inputRotation;

            // speed fall off
            var newVelocity = Vector3.Lerp(state.velocity, Vector3.zero, settings.speedFallOff * dT);

            // accelerate
            var controllerAcceleration = state.rotation * inputAcceleration * settings.acceleration;
            var gravityAcceleration = GravityController.Instance.getGravityAcceleration(state.position);
            newVelocity += (controllerAcceleration + gravityAcceleration) * dT;

            // clamp speed
            newVelocity = Vector3.ClampMagnitude(newVelocity, settings.maxSpeed);
            state.velocity = newVelocity;

            // move
            state.position += newVelocity * dT;

            // shoot
            timeSinceLastShot += dT;
            if (inputShoot && timeSinceLastShot >= settings.attackSpeed) Shoot();
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
