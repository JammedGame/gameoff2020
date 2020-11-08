using UnityEngine;
using Logic;

namespace View
{
    public class FighterView : MonoBehaviour
    {
        public Fighter Fighter { get; set; }

        private void Update()
        {
            transform.localPosition = Fighter.Position;
            transform.localRotation = Fighter.Rotation;
        }
    }
}