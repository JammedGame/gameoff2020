using System;
using System.Collections.Generic;
using UnityEngine;

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
        // public List<MothershipSettings> mothershipSettings;
        // public List<ObjectiveSettings> objectiveSettings;
    }

    [Serializable]
    public class PlanetSettings
    {
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;
        public float mass;
        public GameObject surfacePrefab;
        public Color terrainColor;
        public Color oceanColor;
        public float atmosphereScale;
        public Color atmosphereColor;
        public float outerAtmosphereDensity;
        public float innerAtmosphereDensity;
    }
}