using UnityEngine;

namespace ConjurerDice
{
    public class DiceResolver : MonoBehaviour
    {
        [SerializeField] private NumberResolver numberResolver;
        [SerializeField] private MinionResolver minionResolver;
        [SerializeField] private AbilityResolver abilityResolver;

        public void Resolve(int lane, DiceFaceSO face)
        {
            if (face == null) return;

            switch (face.type)
            {
                case DiceFaceSO.DiceFaceType.Number:
                    numberResolver?.Resolve(lane, face);
                    break;
                case DiceFaceSO.DiceFaceType.Minion:
                    minionResolver?.Resolve(lane, face);
                    break;
                case DiceFaceSO.DiceFaceType.Ability:
                    abilityResolver?.Resolve(lane, face);
                    break;
                case DiceFaceSO.DiceFaceType.Hybrid:
                    // MVP: choose first half if present
                    var half = face.halfA != null ? face.halfA : face.halfB;
                    Resolve(lane, half);
                    break;
            }
        }
    }
}