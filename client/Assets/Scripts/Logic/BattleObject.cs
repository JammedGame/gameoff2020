using Communication;
using Settings;
using UnityEngine;

namespace Logic
{
    public class BattleObject
    {
        public virtual Vector3 Position { get; protected set; }
        public virtual Team Team { get; protected set; }
        public virtual float CurrentHealth { get; protected set; }
        public float CollisionScale { get; protected set; }
        public bool Invulnerable { get; protected set; }
        public bool Dead => !Invulnerable && CurrentHealth <= 0;

        public void TakeDamage(float damage, object source)
        {
            if (Invulnerable) return;

            CurrentHealth = Mathf.Max(0, CurrentHealth - damage);
            Debug.Log($"{this} took {damage} damage from {source}");
        }

        public void GetKilled(object source)
        {
            if (Invulnerable) return;

            CurrentHealth = 0;
            Debug.Log($"{this} was killed by {source}");
        }

        public bool TryCollideWith(WeaponProjectile projectile)
        {
            if (!GameSettings.Instance.FriendlyFireEnabled && projectile.Team == Team) return false;
            if (Vector3.Distance(projectile.Position, Position) > projectile.CollisionScale + CollisionScale) return false;

            TakeDamage(projectile.Damage, projectile.Owner);
            return true;
        }
    }
}