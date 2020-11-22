using System;
using System.Collections.Generic;
using Communication;
using UnityEngine;
using View;

namespace Settings
{
    public enum LevelId
    {
        Undefined = 0,
        LevelOne = 1,
    }

    [Serializable]
    public class LevelSettings
    {
        public LevelId id;
        public List<PlanetSettings> planetSettings;
        public List<MothershipSettings> mothershipSettings;
        // public List<ObjectiveSettings> objectiveSettings;
    }

    [Serializable]
    public class PlanetSettings
    {
        public Vector3 position;
        public Vector3 rotation;
        public Vector3 scale;
        public float mass;
        public Vector3 rotationalVelocity;
        public PlanetView ViewPrefab;
        public Color terrainColor;
        public Color oceanColor;
        public float atmosphereScale;
        public Color atmosphereColor;
        public float outerAtmosphereDensity;
        public float innerAtmosphereDensity;
    }

    public enum MothershipType
    {
        Undefined = 0,
        DefaultMothership = 1,
    }

    [Serializable]
    public class MothershipSettings
    {
        public MothershipType type;
        public Team team;
        public Vector3 position;
        public Vector3 rotationalVelocity;
        public List<DroneSpawnPointSettings> droneSpawnPointSettings;
        public float collisionScale;
        public bool invulnerable;
        public float maxHealth;
    }

    [Serializable]
    public class DroneSpawnPointSettings
    {
        public DroneType droneType;
        public Vector3 relativePosition;
        public float initialSpawnTime;
        public bool periodicSpawn;
        public float spawnPeriod;
    }
}