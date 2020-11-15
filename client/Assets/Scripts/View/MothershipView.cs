using Logic;
using UnityEngine;

namespace View
{
    public class MothershipView : MonoBehaviour
    {
        public Mothership Mothership { get; set; }

        private void Update()
        {
            transform.localPosition = Mothership.Position;
            transform.localRotation = Mothership.Rotation;
        }
    }
}