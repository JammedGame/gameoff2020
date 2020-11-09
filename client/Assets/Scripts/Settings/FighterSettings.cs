using System;

namespace Settings
{
    public enum FighterType
    {
        Undefined = 0,
        BasicFighter = 1,
    }

    [Serializable]
    public class FighterSettings
    {
        public FighterType type;
        public float maxSpeed;
        public float acceleration;
        public float speedFallOff;
        public float attackSpeed;
        public float projectileSpeed;
    }
}
