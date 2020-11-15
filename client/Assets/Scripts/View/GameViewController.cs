using System.Collections.Generic;
using Logic;
using Settings;
using UnityEngine;

namespace View
{
    public class GameViewController : MonoBehaviour
    {
        public CameraController cameraController;

        private readonly Dictionary<Planet, PlanetView> planetViews = new Dictionary<Planet, PlanetView>();
        private readonly Dictionary<Fighter, FighterView> fighterViews = new Dictionary<Fighter, FighterView>();
        private readonly Dictionary<WeaponProjectile, WeaponProjectileView> projectileViews = new Dictionary<WeaponProjectile, WeaponProjectileView>();
        private readonly Dictionary<Mothership, MothershipView> mothershipViews = new Dictionary<Mothership, MothershipView>();
        private readonly Dictionary<Drone, DroneView> droneViews = new Dictionary<Drone, DroneView>();

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

            foreach (var mothership in controller.Motherships)
            {
                UpdateMothershipView(mothership);
            }

            foreach (var drone in controller.Drones)
            {
                UpdateDroneView(drone);
            }
        }

        private void UpdatePlanetView(Planet planet)
        {
            if (!planetViews.TryGetValue(planet, out PlanetView view))
            {
                view = GameObject.Instantiate(planet.Settings.ViewPrefab);
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

        private void UpdateMothershipView(Mothership mothership)
        {
            if (!mothershipViews.TryGetValue(mothership, out MothershipView view))
            {
                view = mothership.Type.LoadView();
                view.Mothership = mothership;
                mothershipViews.Add(mothership, view);
            }
        }

        private void UpdateDroneView(Drone drone)
        {
            if (!droneViews.TryGetValue(drone, out DroneView view))
            {
                view = drone.Type.LoadView();
                view.Drone = drone;
                droneViews.Add(drone, view);
            }
        }
    }
}