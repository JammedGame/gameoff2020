using Logic;
using UnityEngine;

namespace View
{
    public class WeaponProjectileView : MonoBehaviour
    {
        public float smoothMovement = 0.8f;

        public WeaponProjectile WeaponProjectile { get; set; }

        private void Start()
        {
            transform.localPosition = WeaponProjectile.Position;
            transform.localRotation = WeaponProjectile.Rotation;
        }

        private void Update()
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, WeaponProjectile.Position, 1 - smoothMovement);
            transform.localRotation = WeaponProjectile.Rotation;
        }
    }
}