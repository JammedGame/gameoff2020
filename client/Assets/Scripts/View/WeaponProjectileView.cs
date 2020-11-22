using Logic;
using UnityEngine;

namespace View
{
    public class WeaponProjectileView : BattleView<WeaponProjectileView>
    {
        public float smoothMovement = 0.8f;
        public WeaponProjectile WeaponProjectile { get; private set; }

        public static WeaponProjectileView Create(WeaponProjectile weaponProjectile)
        {
            var newView = FetchFromPool();
            if (newView == null) newView = GameObject.Instantiate(Resources.Load<WeaponProjectileView>("Prefabs/WeaponProjectileView"));
            newView.WeaponProjectile = weaponProjectile;
            newView.transform.localPosition = weaponProjectile.Position;
            newView.transform.localRotation = weaponProjectile.Rotation;
            return newView;
        }

        private void Update()
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, WeaponProjectile.Position, 1 - smoothMovement);
            transform.localRotation = WeaponProjectile.Rotation;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Planet")) 
            {
                var planet = other.GetComponentInParent<PlanetView>()?.Planet;
                WeaponProjectile.GetKilled(planet);
            }
            else if (other.CompareTag("Mothership"))
            {
                var mothership = other.GetComponentInParent<MothershipView>()?.Mothership;
                WeaponProjectile.GetKilled(mothership);
            }
        }
    }
}