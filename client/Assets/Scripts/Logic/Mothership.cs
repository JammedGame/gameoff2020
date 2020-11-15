using System.Collections.Generic;
using System.Linq;
using Settings;
using UnityEngine;

namespace Logic
{
    public class Mothership
    {
        private readonly List<DroneSpawnPoint> droneSpawnPoints = new List<DroneSpawnPoint>();

        public MothershipSettings Settings { get; private set; }
        public MothershipType Type => Settings.type;
        public Allegiance Allegiance => Settings.allegiance;
        public Vector3 Position => Settings.position;
        public Quaternion Rotation { get; private set; }

        public Mothership(MothershipSettings settings)
        {
            Settings = settings;
            droneSpawnPoints.AddRange(settings.droneSpawnPointSettings.Select(s => new DroneSpawnPoint(s, Allegiance, settings.position)));
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

        public DroneSpawnPointSettings Settings { get; private set; }
        public Allegiance Allegiance { get; private set; }
        public Vector3 SpawnPosition { get; private set; }

        public DroneSpawnPoint(DroneSpawnPointSettings settings, Allegiance allegiance, Vector3 mothershipPosition)
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