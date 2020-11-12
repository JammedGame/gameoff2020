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
        public float Radius => Settings.scale.x * 0.5f;
        public float RadiusIncludingAtmosphere => Radius * Settings.atmosphereScale;
        public float AtmosphereHeight => RadiusIncludingAtmosphere - Radius;
        public Quaternion Rotation { get; private set; }

        public Planet(PlanetSettings settings)
            => (this.settings, Rotation) = (settings, Quaternion.Euler(settings.rotation));

        public void Tick(float dT)
        {
            Rotation *= Quaternion.Euler(settings.rotationalVelocity * dT);
        }
    }
}