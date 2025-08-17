using UnityEngine;

namespace ConjurerDice
{
    public class AbilityService : MonoBehaviour
    {
        public bool TryCast(AbilitySO ability, AbilityCastContext ctx)
        {
            if (ability == null || ability.effect == null) return false;

            // Minimal validity check (lane constrained elsewhere)
            ability.effect.Apply(ctx);
#if UNITY_EDITOR
            Debug.Log($"[AbilityService] Cast {ability.name} (mag {ability.baseMagnitude}) in lane {ctx.lane}");
#endif
            return true;
        }
    }
}