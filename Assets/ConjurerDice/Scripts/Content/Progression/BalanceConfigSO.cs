// Scripts/Content/Progression/BalanceConfigSO.cs
using UnityEngine;

namespace ConjurerDice
{
    [CreateAssetMenu(menuName = "ConjurerDice/Balance/Chapter Balance")]
    public class BalanceConfigSO : ScriptableObject
    {
        public int startingMana = 3;  // Encounter start value (resets every encounter)
        public int manaPerTurn = 1;   // Fixed regen each Start phase
        public int relicBonusStart = 0; // optional: persistent bonus from relics
        public int relicBonusPerTurn = 0;
    }
}