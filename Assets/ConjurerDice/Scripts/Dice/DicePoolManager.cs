using UnityEngine;
using System.Collections.Generic;

namespace ConjurerDice
{
    /// Active pool for the current encounter (expanded faces, roll API).
    public class DicePoolManager : MonoBehaviour
    {
        [SerializeField] private PlayerDiceLoadoutSO playerLoadout;

        private readonly List<DiceDefinitionSO> _activeDice = new();
        private readonly System.Random _rng = new System.Random();

        public void BuildEncounterPool()
        {
            _activeDice.Clear();
            _activeDice.AddRange(playerLoadout.chosenDice);
        }

        public (DiceDefinitionSO die, DiceFaceSO face) RollOne()
        {
            if (_activeDice.Count == 0) return (null, null);

            int dieIndex = _rng.Next(0, _activeDice.Count);
            var die = _activeDice[dieIndex];
            if (die == null || die.faces == null || die.faces.Length == 0) return (null, null);

            int faceIndex = _rng.Next(0, die.faces.Length);
            return (die, die.faces[faceIndex]);
        }

        public List<DiceDefinitionSO> GetActiveDice() => _activeDice;
    }
}