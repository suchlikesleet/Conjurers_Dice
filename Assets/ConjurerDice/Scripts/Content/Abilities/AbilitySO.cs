using UnityEngine;

namespace ConjurerDice {
    [CreateAssetMenu(menuName="ConjurerDice/AbilitySO")]
    public class AbilitySO : ScriptableObject {

        public enum TargetKind { UnitAlly, UnitEnemy, Tile, Multi }
        public enum Shape { Single, Line, Cross, ConeForward, LaneAoE }

        [Header("Targeting")]
        public TargetKind targetKind = TargetKind.UnitEnemy;
        [Min(0)] public int minRange = 0;           // tiles from caster lane index
        [Min(0)] public int maxRange = 3;           // inclusive
        public Shape shape = Shape.Single;
        public int shapeRadius = 0;                  // for Cross/Cone/LaneAoE
        public bool requireSameLane = true;          // if true, lock to lane (you already do this)
        public bool allowMultiple = false;           // multi-select confirmation
        public int maxTargets = 1;                   // cap multi-select
        public bool lineBlocksAtFirstUnit = true; 

        public int baseMagnitude = 1;
        public int rangeTiles = 1;
        public StatusEffectSO appliedStatus;
        public AbilityEffectSO effect;
        public int manaModifier = 0;

    }
}
