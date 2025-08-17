// Assets/ConjurerDice/Scripts/Dice/Customize/LoadoutValidator.cs
using UnityEngine;

namespace ConjurerDice
{
    public class LoadoutValidator : MonoBehaviour
    {
        [SerializeField] private DiceConstraintSO[] constraints;
        [Min(0)][SerializeField] private int minDiceToConfirm = 1;   // allow fewer than max; set 0 if you want

        public string CanAdd(PlayerDiceLoadoutSO loadout, DiceDefinitionSO candidateToAdd)
        {
            if (loadout == null || candidateToAdd == null) return "Invalid die.";
            foreach (var c in constraints)
            {
                var why = c?.Validate(loadout, candidateToAdd);
                if (!string.IsNullOrEmpty(why)) return why;
            }
            return null;
        }

        public string CanConfirm(PlayerDiceLoadoutSO loadout)
        {
            if (loadout == null) return "No loadout.";
            if (loadout.chosenDice.Count < minDiceToConfirm)
                return $"Choose at least {minDiceToConfirm} die{(minDiceToConfirm>1?"s":"")}.";

            foreach (var c in constraints)
            {
                var why = c?.ValidateFinal(loadout);
                if (!string.IsNullOrEmpty(why)) return why;
            }
            return null;
        }
    }
}