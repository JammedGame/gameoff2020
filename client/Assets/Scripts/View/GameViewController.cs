using System.Collections.Generic;
using Logic;
using UnityEngine;

namespace View
{
    public class GameViewController : MonoBehaviour
    {
        public CameraController cameraController;

        private readonly Dictionary<Planet, PlanetView> planetViews = new Dictionary<Planet, PlanetView>();
        private readonly Dictionary<Fighter, FighterView> fighterViews = new Dictionary<Fighter, FighterView>();
        private readonly Dictionary<WeaponProjectile, WeaponProjectileView> projectileViews = new Dictionary<WeaponProjectile, WeaponProjectileView>();

        public void UpdateViews(GameController controller)
        {
            foreach (var planet in controller.Planets)
            {
                UpdatePlanetView(planet);
            }

            foreach (var player in controller.Fighters)
            {
                UpdateFighterView(player);
            }

            foreach (var projectile in controller.Projectiles)
            {
                UpdateProjectileView(projectile);
            }
        }

        private void UpdatePlanetView(Planet planet)
        {
            if (!planetViews.TryGetValue(planet, out PlanetView view))
            {
                view = GameObject.Instantiate(Resources.Load<PlanetView>("Prefabs/PlanetView"));
                view.Planet = planet;
                planetViews.Add(planet, view);
            }
        }

        private void UpdateFighterView(Fighter fighter)
        {
            if (!fighterViews.TryGetValue(fighter, out FighterView view))
            {
                view = GameObject.Instantiate(Resources.Load<FighterView>("Prefabs/FighterView"));
                view.Fighter = fighter;
                fighterViews.Add(fighter, view);

                if (fighter.IsPlayer) cameraController.target = view;
            }
        }

        private void UpdateProjectileView(WeaponProjectile projectile)
        {
            if (!projectileViews.TryGetValue(projectile, out WeaponProjectileView view))
            {
                view = GameObject.Instantiate(Resources.Load<WeaponProjectileView>("Prefabs/WeaponProjectileView"));
                view.WeaponProjectile = projectile;
                projectileViews.Add(projectile, view);
            }
        }
    }
}