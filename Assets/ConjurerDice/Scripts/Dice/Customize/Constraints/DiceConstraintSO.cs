using UnityEngine;

namespace ConjurerDice
{
    [CreateAssetMenu(menuName = "ConjurerDice/Dice Constraint")]
    public abstract class DiceConstraintSO : ScriptableObject
    {
        [Header("Mandatory Rules")]
        [Tooltip("Minimum number faces required on the die (always enforced).")]
        public int minNumberFaces = 1;

        [Header("Max Limits")]
        [Tooltip("Maximum number of ability faces allowed on this die.")]
        public int maxAbilityFaces = 2;

        [Tooltip("Maximum number of minion faces allowed on this die.")]
        public int maxMinionFaces = 3;

        [Tooltip("Maximum number of hybrid faces allowed on this die.")]
        public int maxHybridFaces = 2;

        [Header("Optional Flags")]
        [Tooltip("Allow dice with empty/blank faces? (fizzles when rolled).")]
        public bool allowEmptyFaces = true;
        
        
        /// Called when the user tries to ADD one die.
        /// Return null if allowed, otherwise a human-readable reason (shown to the player).
        public abstract string Validate(PlayerDiceLoadoutSO loadout, DiceDefinitionSO candidateToAdd);

        /// Called when the user presses CONFIRM.
        /// Return null if the whole loadout is valid, otherwise a reason.
        public virtual string ValidateFinal(PlayerDiceLoadoutSO loadout) => null;
    }
}