using System;

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
        public float defaultSpeed;
        public float boostSpeed;
        public float brakeSpeed;
        public float velocitySmooth;
        public float steeringSpeed;
        public float attackSpeed;
        public float projectileSpeed;
    }
}
