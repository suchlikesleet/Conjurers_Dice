// Scripts/Dice/Customize/Constraints/MaxPerTagConstraint.cs
using UnityEngine;
using System.Linq;

namespace ConjurerDice
{
    [CreateAssetMenu(menuName="ConjurerDice/Dice Constraints/Max Per Tag")]
    public class MaxPerTagConstraint : DiceConstraintSO
    {
        [System.Serializable] public class Rule { public string tag; [Min(0)] public int max = 2; }
        [Tooltip("For each listed tag, the loadout can include at most 'max' dice having that tag.")]
        public Rule[] rules;

        public override string Validate(PlayerDiceLoadoutSO loadout, DiceDefinitionSO candidateToAdd)
        {
            if (rules == null || candidateToAdd == null) return null;
            foreach (var r in rules)
            {
                if (string.IsNullOrWhiteSpace(r.tag)) continue;
                bool candidateHas = candidateToAdd.tags != null && candidateToAdd.tags.Contains(r.tag);
                if (!candidateHas) continue;

                int current = loadout.chosenDice.Count(d => d != null && d.tags != null && d.tags.Contains(r.tag));
                int after   = current + 1;
                if (after > r.max)
                    return $"At most {r.max} {r.tag} dice allowed.";
            }
            return null;
        }

        public override string ValidateFinal(PlayerDiceLoadoutSO loadout)
        {
            if (rules == null) return null;
            foreach (var r in rules)
            {
                if (string.IsNullOrWhiteSpace(r.tag)) continue;
                int count = loadout.chosenDice.Count(d => d != null && d.tags != null && d.tags.Contains(r.tag));
                if (count > r.max) return $"At most {r.max} {r.tag} dice allowed.";
            }
            return null;
        }
    }
}