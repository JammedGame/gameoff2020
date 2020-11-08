using Logic;
using UnityEngine;

namespace View
{
    public class WeaponProjectileView : MonoBehaviour 
    {
        public WeaponProjectile WeaponProjectile { get; set; }

        private void Update()
        {
            transform.localPosition = WeaponProjectile.Position;
            transform.localRotation = WeaponProjectile.Rotation;
        }
    }
}