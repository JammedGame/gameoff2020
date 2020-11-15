using System;
using System.Collections.Generic;
using UnityEngine;

namespace Settings
{
    public enum FighterType
    {
        Undefined = 0,
        Mosquito = 1,
    }

    [Serializable]
    public class FighterSettings
    {
        public FighterType type;
        public float maxHealth;
        public float defaultSpeed;
        public float boostSpeed;
        public float brakeSpeed;
        public AnimationCurve steerSpeedCurve;
        public float rollSpeed;
        public float attackDamage;
        public float attackSpeed;
        public float projectileSpeed;
        public List<Vector3> turrets;
    }
}
