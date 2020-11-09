using Settings;
using UnityEngine;

namespace Logic
{
    public class Planet
    {
        private PlanetSettings settings;

        public PlanetSettings Settings => settings;
        public Vector3 Position => settings.position;
        public float Mass => settings.mass;

        public Planet(PlanetSettings settings) => this.settings = settings;
    }
}