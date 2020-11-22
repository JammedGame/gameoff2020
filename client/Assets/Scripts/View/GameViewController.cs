using System.Collections.Generic;
using System.Linq;
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
        private readonly Dictionary<Mothership, MothershipView> mothershipViews = new Dictionary<Mothership, MothershipView>();
        private readonly Dictionary<Drone, DroneView> droneViews = new Dictionary<Drone, DroneView>();

        public void UpdateViews(GameController controller)
        {
            foreach (var planet in controller.Planets)
            {
                UpdatePlanetView(planet);
            }

            foreach (var fighter in controller.Fighters)
            {
                UpdateFighterView(fighter);
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

            CleanUpDeadViews();
        }

        private void UpdatePlanetView(Planet planet)
        {
            if (!planetViews.TryGetValue(planet, out PlanetView view))
            {
                view = PlanetView.Create(planet);
                planetViews.Add(planet, view);
            }
        }

        private void UpdateFighterView(Fighter fighter)
        {
            if (!fighterViews.TryGetValue(fighter, out FighterView view))
            {
                view = FighterView.Create(fighter);
                fighterViews.Add(fighter, view);

                if (fighter.IsPlayer) cameraController.target = view;
            }
        }

        private void UpdateProjectileView(WeaponProjectile projectile)
        {
            if (!projectileViews.TryGetValue(projectile, out WeaponProjectileView view))
            {
                view = WeaponProjectileView.Create(projectile);
                projectileViews.Add(projectile, view);
            }
        }

        private void UpdateMothershipView(Mothership mothership)
        {
            if (!mothershipViews.TryGetValue(mothership, out MothershipView view))
            {
                view = MothershipView.Create(mothership);
                mothershipViews.Add(mothership, view);
            }
        }

        private void UpdateDroneView(Drone drone)
        {
            if (!droneViews.TryGetValue(drone, out DroneView view))
            {
                view = DroneView.Create(drone);
                droneViews.Add(drone, view);
            }
        }

        private void CleanUpDeadViews()
        {
            foreach (var fighter in fighterViews.Where(v => v.Key.Dead).ToList())
            {
                var view = fighter.Value;
                fighterViews.Remove(fighter.Key);
                view.ReturnToPool();
            }

            foreach (var projectile in projectileViews.Where(v => v.Key.Dead).ToList())
            {
                var view = projectile.Value;
                projectileViews.Remove(projectile.Key);
                view.ReturnToPool();
            }

            foreach (var drone in droneViews.Where(v => v.Key.Dead).ToList())
            {
                var view = drone.Value;
                droneViews.Remove(drone.Key);
                view.ReturnToPool();
            }
        }
    }
}