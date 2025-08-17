using UnityEngine;

namespace ConjurerDice
{
    public abstract class AbilityEffectSO : ScriptableObject
    {
        public abstract void Apply(AbilityCastContext ctx);
    }
}