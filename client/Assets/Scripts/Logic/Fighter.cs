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

    public class Fighter : BattleObject
    {
        private FighterSettings settings;
        private PlayerState state;
        private float timeSinceLastShot;
        private FighterInput currentInput;

        public PlayerState State => state;
        public string PlayerId => state.id;
        public override Team Team => state.team;
        public override float CurrentHealth
        {
            get => state.currentHealth;
            protected set => state.currentHealth = value;
        }
        public bool IsDead => CurrentHealth <= 0;
        public bool IsPlayer => PlayerId == GameController.Instance.PlayerState.id;
        public override Vector3 Position => state.position;
        public Quaternion Rotation => state.rotation;
        public Vector3 Velocity => state.velocity;
        public override float CollisionScale => settings.collisionScale;
        public Planet IsInAtmosphereOfPlanet => GameController.Instance.IsInAtmosphereOfPlanet(Position);

        public Fighter(FighterSettings settings, PlayerState state)
        {
            this.settings = settings;
            this.state = state;
            this.state.currentHealth = settings.maxHealth;
        }

        public void SetPlayerInput(FighterInput input) => currentInput = input;

        public void Tick(float dT)
        {
            state = Simulate(state, currentInput, dT);

            if (currentInput.Shoot && timeSinceLastShot >= settings.attackSpeed)
            {
                Shoot(currentInput.ShootDirection);
            }
        }

        public PlayerState Simulate(PlayerState previousState, FighterInput input, float dT)
        {
            var state = new PlayerState();
            state.CopyFrom(previousState);

            // rotate
            var newRotation = state.rotation;
            var steerSpeedX = -input.SteerTarget.y;
            var steerSpeedY = input.SteerTarget.x;
            steerSpeedX = Mathf.Sign(steerSpeedX) * settings.steerSpeedCurve.Evaluate(Mathf.Abs(steerSpeedX));
            steerSpeedY = Mathf.Sign(steerSpeedY) * settings.steerSpeedCurve.Evaluate(Mathf.Abs(steerSpeedY));
            newRotation *= Quaternion.Euler(
                steerSpeedX * dT,
                steerSpeedY * dT,
                -input.Roll * settings.rollSpeed * dT
            );
            state.rotation = newRotation;

            // velocity
            var newVelocity = state.velocity;
            if (!input.Drift)
            {
                var newSpeed = input.Throttle >= 0
                    ? Mathf.Lerp(settings.defaultSpeed, settings.boostSpeed, input.Throttle)
                    : Mathf.Lerp(settings.defaultSpeed, settings.brakeSpeed, -input.Throttle);
                newVelocity = newRotation * Vector3.forward * newSpeed;
                state.velocity = newVelocity;
            }

            // move
            state.position += newVelocity * dT;

            // shoot
            timeSinceLastShot += dT;
            return state;
        }

        private void Shoot(Quaternion shootDirection)
        {
            timeSinceLastShot = 0;

            var projectileRotation = state.rotation * shootDirection;

            foreach(var turret in settings.turrets)
            {
                var projectile = new WeaponProjectile(
                    this,
                    settings.attackDamage,
                    state.position + state.rotation * turret,
                    projectileRotation,
                    state.velocity + projectileRotation * Vector3.forward * settings.projectileSpeed
                );
                GameController.Instance.AddProjectile(projectile);
            }
        }
    }
}
