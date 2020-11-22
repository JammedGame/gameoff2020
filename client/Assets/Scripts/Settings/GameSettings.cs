using System;
using System.Collections.Generic;
using Communication;
using UnityEngine;
using View;

namespace Settings
{
    public class GameSettings : ScriptableObject
    {
        private static GameSettings instance;
        public static GameSettings Instance {
            get
            {
                if (instance == null) instance = Resources.Load<GameSettings>("GameSettings");
                return instance;
            }
        }

        public float GravityCoefficient;
        public bool FriendlyFireEnabled;
        public WeaponProjectileSettings WeaponProjectileSettings;
        public List<LevelSettings> LevelSettings;
        public List<FighterSettings> FighterSettings;
        public List<DroneSettings> DroneSettings;
    }

    [Serializable]
    public class WeaponProjectileSettings
    {
        public float collisionScale;
    }

    public enum DroneType
    {
        Undefined = 0,
        Defensive = 1,
        Scout = 2,
        Offensive = 3,
    }

    [Serializable]
    public class DroneSettings
    {
        public DroneType type;
        public float maxHealth;
        public float maxSpeed;
        public float attackDamage;
        public float attackSpeed;
        public float attackRange;
        public float projectileSpeed;
        public float collisionScale;
    }

    public static class SettingsExtensions
    {
        public static Team GetOpponent(this Team team)
        {
            switch (team)
            {
                case Team.Blue: return Team.Red;
                case Team.Red: return Team.Blue;
                default: return Team.Neutral;
            }
        }

        public static LevelSettings GetSettings(this LevelId id)
        {
            return GameSettings.Instance.LevelSettings.Find(f => f.id == id);
        }

        public static FighterSettings GetSettings(this FighterType type)
        {
            return GameSettings.Instance.FighterSettings.Find(f => f.type == type);
        }

        public static DroneSettings GetSettings(this DroneType type)
        {
            return GameSettings.Instance.DroneSettings.Find(d => d.type == type);
        }

        public static DroneView LoadView(this DroneType type)
        {
            return GameObject.Instantiate(Resources.Load<DroneView>($"Prefabs/Drones/{type}"));
        }

        public static MothershipView LoadView(this MothershipType type)
        {
            return GameObject.Instantiate(Resources.Load<MothershipView>($"Prefabs/Motherships/{type}"));
        }
    }
}
