using UnityEngine;
using Logic;

namespace View
{
    public class FighterView : MonoBehaviour
    {
        public float smoothMovement = 0.8f;

        public Fighter Fighter { get; set; }

        private void Start()
        {
            transform.localPosition = Fighter.Position;
        }

        private void Update()
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, Fighter.Position, 1 - smoothMovement);
            transform.localRotation = Fighter.Rotation;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Planet")) Fighter.TakeDamage(float.MaxValue, null);
        }
    }
}