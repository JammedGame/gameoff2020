using Logic;
using UnityEngine;

namespace View
{
    public class WeaponProjectileView : MonoBehaviour
    {
        public float smoothMovement = 0.8f;
        public WeaponProjectile WeaponProjectile { get; private set; }

        public static WeaponProjectileView Create(WeaponProjectile weaponProjectile)
        {
            var newView = GameObject.Instantiate(Resources.Load<WeaponProjectileView>("Prefabs/WeaponProjectileView"));
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
    }
}