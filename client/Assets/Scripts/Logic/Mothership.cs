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
        public Quaternion Rotation { get; private set; }

        public Mothership(MothershipSettings settings)
        {
            Settings = settings;
            Position = settings.position;
            Rotation = Quaternion.Euler(settings.rotation);
            Team = settings.team;
            CurrentHealth = settings.maxHealth;
            Invulnerable = settings.invulnerable;
            droneSpawnPoints.AddRange(settings.droneSpawnPointSettings.Select(s => new DroneSpawnPoint(s, this)));
        }

        public void Tick(float dT)
        {
            Rotation *= Quaternion.Euler(Settings.rotationalVelocity * dT);
            foreach (var spawnPoint in droneSpawnPoints)
            {
                spawnPoint.Tick(dT);
            }
        }
    }

    public class DroneSpawnPoint
    {
        private float timeUntilNextSpawn;
        private bool initialSpawnCompleted;
        private Mothership owner;

        public DroneSpawnPointSettings Settings { get; private set; }
        public Team Team { get; private set; }
        public Vector3 SpawnPosition { get; private set; }
        public Quaternion SpawnRotation { get; private set; }

        public DroneSpawnPoint(DroneSpawnPointSettings settings, Mothership owner)
            => (Settings, this.owner, Team, timeUntilNextSpawn) = (settings, owner, owner.Team, settings.initialSpawnTime);

        public void Tick(float dT)
        {
            SpawnPosition = owner.Position + owner.Rotation * Settings.relativePosition;
            SpawnRotation = owner.Rotation * Quaternion.Euler(Settings.relativeRotation);

            timeUntilNextSpawn -= dT;
            if (timeUntilNextSpawn <= 0 && (!initialSpawnCompleted || Settings.periodicSpawn))
            {
                var drone = new Drone(Settings.droneType.GetSettings(), Team, SpawnPosition, SpawnRotation);
                GameController.Instance.AddDrone(drone);
                initialSpawnCompleted = true;
                timeUntilNextSpawn += Settings.spawnPeriod;
            }
        }
    }
}