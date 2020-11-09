using System.Collections.Generic;
using UnityEngine;

namespace Settings
{
    public class GameSettings : ScriptableObject
    {
        private static GameSettings instance;
        public static GameSettings Instance {
            get {
                if (instance == null) instance = Resources.Load<GameSettings>("GameSettings");
                return instance;
            }
        }

        public float GravityCoefficient;
        public List<LevelSettings> LevelSettings;
        public List<FighterSettings> FighterSettings;
    }

    public static class SettingsExtensions
    {
        public static LevelSettings GetSettings(this LevelId id)
        {
            return GameSettings.Instance.LevelSettings.Find(f => f.id == id);
        }

        public static FighterSettings GetSettings(this FighterType type)
        {
            return GameSettings.Instance.FighterSettings.Find(f => f.type == type);
        }
    }
}
