using UnityEngine;
using System.Collections.Generic;

namespace ConjurerDice
{
    [CreateAssetMenu(menuName="ConjurerDice/Dice/Player Dice Loadout")]
    public class PlayerDiceLoadoutSO : ScriptableObject
    {
        [Tooltip("Chosen dice (definitions). Order matters for UI display.")]
        public List<DiceDefinitionSO> chosenDice = new();

        [Tooltip("Max dice the player can bring into an encounter.")]
        public int maxDice = 3;

        public void Clear() => chosenDice.Clear();

        public bool TryAdd(DiceDefinitionSO def)
        {
            if (def == null) return false;
            if (chosenDice.Count >= maxDice) return false;
            int copies = chosenDice.FindAll(d => d == def).Count;
            if (def.maxCopiesInLoadout > 0 && copies >= def.maxCopiesInLoadout) return false;
            chosenDice.Add(def);
            return true;
        }

        public void RemoveAt(int idx)
        {
            if (idx >= 0 && idx < chosenDice.Count)
                chosenDice.RemoveAt(idx);
        }
    }
}