using Communication;
using Settings;
using UnityEngine;

namespace Logic
{
    public class BattleObject
    {
        public virtual Vector3 Position
        {
            get => throw new System.NotImplementedException();
            protected set => throw new System.NotImplementedException();
        }
        public virtual Team Team
        {
            get => throw new System.NotImplementedException();
            protected set => throw new System.NotImplementedException();
        }
        public virtual float CurrentHealth
        {
            get => throw new System.NotImplementedException();
            protected set => throw new System.NotImplementedException();
        }
        public virtual float CollisionScale
        {
            get => throw new System.NotImplementedException();
            protected set => throw new System.NotImplementedException();
        }

        public virtual void TakeDamage(float damage, WeaponProjectile source)
        {
            CurrentHealth -= damage;
            Debug.Log($"{this.GetType()} took {damage} damage from {source?.Owner.GetType()}");
        }

        public bool TryCollideWith(WeaponProjectile projectile)
        {
            if (!GameSettings.Instance.FriendlyFireEnabled && projectile.Team == Team) return false;
            if (Vector3.Distance(projectile.Position, Position) > projectile.CollisionScale + CollisionScale) return false;
            TakeDamage(projectile.Damage, projectile);
            return true;
        }
    }
}