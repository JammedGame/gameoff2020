using Logic;
using UnityEngine;

namespace View
{
    public class DroneView : MonoBehaviour
    {
        public Drone Drone { get; set; }

        private void Update()
        {
            transform.localPosition = Drone.Position;
            transform.localRotation = Drone.Rotation;
        }
    }
}