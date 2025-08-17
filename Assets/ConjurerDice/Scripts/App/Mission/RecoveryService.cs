using UnityEngine;

namespace ConjurerDice {
    public class RecoveryService : MonoBehaviour {
        [SerializeField] private RunEffectsService runEffects;

        private void Awake()
        {
            if (runEffects == null) runEffects = FindFirstObjectByType<RunEffectsService>();
        }

        // Mana Spring between encounters → adds a cross-encounter buff
        public void GrantManaSpring(int perTurnAmount, int turns)
        {
            runEffects?.AddManaPerTurn(perTurnAmount, turns); // e.g., +1 for 2 turns
        }

        // Curses work the same with negative deltas
        public void ApplyManaDrought(int perTurnPenalty, int turns)
        {
            if (perTurnPenalty <= 0) return;           // pass +1 to mean “-1 per turn”
            runEffects?.AddManaPerTurn(-perTurnPenalty, turns);
        }
        public void HealAll() {{ /* TODO */ }}
        public void BoostMana(int turns) {{ /* TODO */ }}
        public void CleanseCurse() {{ /* TODO */ }}

    }
}
