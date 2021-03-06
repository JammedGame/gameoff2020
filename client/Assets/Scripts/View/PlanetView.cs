using Logic;
using UnityEngine;

namespace View
{
    public class PlanetView : BattleView<PlanetView>
    {
        public MeshRenderer atmosphere;

        public MeshRenderer surface;

        public Planet Planet { get; set; }

        public static PlanetView Create(Planet planet)
        {
            var newView = FetchFromPool();
            if (newView == null) newView = GameObject.Instantiate(planet.Settings.ViewPrefab);
            newView.Planet = planet;
            newView.Init();
            return newView;
        }

        private void Init()
        {
            var t = transform;
            t.localPosition = Planet.Settings.position;
            t.localRotation = Planet.Rotation;
            t.localScale = Planet.Settings.scale;

            var atmosphereHeight = Planet.AtmosphereHeight;
            //if (surface.materials.Length > 0)
            //{
            //    var ocean = surface.materials[0];
            //    ocean.SetColor("_Color", Planet.Settings.oceanColor);
            //    ocean.SetColor("_AtmosphereTint", Planet.Settings.atmosphereColor);
            //    ocean.SetFloat("_AtmosphereAlpha", Planet.Settings.innerAtmosphereDensity);
            //    ocean.SetFloat("_AtmosphereHeight", atmosphereHeight);
            //}
            //if (surface.materials.Length > 1)
            //{
            //    var terrain = surface.materials[1];
            //    terrain.SetColor("_Color", Planet.Settings.terrainColor);
            //    terrain.SetColor("_AtmosphereTint", Planet.Settings.atmosphereColor);
            //    terrain.SetFloat("_AtmosphereAlpha", Planet.Settings.innerAtmosphereDensity);
            //    terrain.SetFloat("_AtmosphereHeight", atmosphereHeight);
            //}

            var atmosphereTransform = atmosphere.transform;
            atmosphereTransform.localScale = Planet.Settings.atmosphereScale * Vector3.one;

            var atmosphereMat = atmosphere.material;
            //atmosphereMat.SetColor("_Tint", Planet.Settings.atmosphereColor);
            //atmosphereMat.SetFloat("_FrontAlpha", Planet.Settings.outerAtmosphereDensity);
            //atmosphereMat.SetFloat("_BackAlpha", Planet.Settings.outerAtmosphereDensity);
        }

        private void Update()
        {
            transform.localRotation = Planet.Rotation;
        }
    }
}