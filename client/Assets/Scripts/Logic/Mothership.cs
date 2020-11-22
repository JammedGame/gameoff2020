using System.Collections.Generic;
using System.Linq;
using Communication;
using Settings;
using UnityEngine;

namespace Logic
{
    public class Mothership : BattleObject
    {
        private readonly List<DroneSpawnPoint> droneSpawnPoints = new List<DroneSpawnPoint>();

        public MothershipSettings Settings { get; private set; }
        public MothershipType Type => Settings.type;
        public override Team Team => Settings.team;
        public override Vector3 Position => Settings.position;
        public Quaternion Rotation { get; private set; }
        public override float CollisionScale => Settings.collisionScale;

        public Mothership(MothershipSettings settings)
        {
            Settings = settings;
            droneSpawnPoints.AddRange(settings.droneSpawnPointSettings.Select(s => new DroneSpawnPoint(s, Team, settings.position)));
        }

        public void Tick(float dT)
        {
            Rotation *= Quaternion.Euler(Settings.rotationalVelocity * dT);
            foreach (var spawnPoint in droneSpawnPoints)
            {
                spawnPoint.Tick(dT);
            }
        }

        public override void TakeDamage(float damage, WeaponProjectile source)
        {
            Debug.Log($"{this.GetType()} took {damage} damage from {source.Owner.GetType()}");
        }
    }

    public class DroneSpawnPoint
    {
        private float timeUntilNextSpawn;
        private bool initialSpawnCompleted;

        public DroneSpawnPointSettings Settings { get; private set; }
        public Team Allegiance { get; private set; }
        public Vector3 SpawnPosition { get; private set; }

        public DroneSpawnPoint(DroneSpawnPointSettings settings, Team allegiance, Vector3 mothershipPosition)
            => (Settings, Allegiance, SpawnPosition, timeUntilNextSpawn) = (settings, allegiance, mothershipPosition + settings.relativePosition, settings.initialSpawnTime);

        public void Tick(float dT)
        {
            timeUntilNextSpawn -= dT;
            if (timeUntilNextSpawn <= 0 && (!initialSpawnCompleted || Settings.periodicSpawn))
            {
                var drone = new Drone(Settings.droneType.GetSettings(), Allegiance, SpawnPosition);
                GameController.Instance.AddDrone(drone);
                initialSpawnCompleted = true;
                timeUntilNextSpawn += Settings.spawnPeriod;
            }
        }
    }
}