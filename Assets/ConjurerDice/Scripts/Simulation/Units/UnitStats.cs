using UnityEngine;

namespace ConjurerDice {
    [System.Serializable]
    public struct UnitStats {
        public int HP;
        public int MaxHP;
        public int ATK;
        public int DEF;
        public MovementType MOVType;
        public int FixedMov;
        public int MovCap;
        public int RNG;
    }
    public enum MovementType { Fixed, Standard, Slow, Fast, Capped }
}
