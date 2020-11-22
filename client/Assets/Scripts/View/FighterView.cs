﻿using UnityEngine;
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
            if (other.CompareTag("Planet")) 
            {
                var planet = other.GetComponentInParent<PlanetView>()?.Planet;
                Fighter.GetKilled(planet);
            }
            else if (other.CompareTag("Mothership"))
            {
                var mothership = other.GetComponentInParent<MothershipView>()?.Mothership;
                Fighter.GetKilled(mothership);
            }
        }
    }
}