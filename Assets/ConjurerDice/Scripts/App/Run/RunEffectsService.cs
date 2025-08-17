using UnityEngine;
using System.Collections.Generic;

namespace ConjurerDice
{
    /// <summary>
    /// Holds temporary cross-encounter effects with remaining turns.
    /// Example: +2 mana per turn for 3 turns (carryover if encounter ends early).
    /// </summary>
    public class RunEffectsService : MonoBehaviour
    {
        [System.Serializable]
        public struct ManaPerTurnEffect
        {
            public int delta;          // +2 or -1
            public int remainingTurns; // decremented on each Start phase (any encounter)
        }

        [SerializeField] private List<ManaPerTurnEffect> manaTurnEffects = new();

        // --- Public API ---
        public void AddManaPerTurn(int delta, int turns)
        {
            if (turns <= 0 || delta == 0) return;
            manaTurnEffects.Add(new ManaPerTurnEffect { delta = delta, remainingTurns = turns });
#if UNITY_EDITOR
            Debug.Log($"[RunEffects] Added mana/turn {delta} for {turns} turns.");
#endif
            Save();
        }

        public int GetActiveManaPerTurnDelta()
        {
            int sum = 0;
            for (int i = 0; i < manaTurnEffects.Count; i++) sum += manaTurnEffects[i].delta;
            return sum;
        }

        /// <summary>Call once per Start phase (of any encounter) AFTER applying regen.</summary>
        public void DecayOneTurn()
        {
            for (int i = manaTurnEffects.Count - 1; i >= 0; i--)
            {
                var e = manaTurnEffects[i];
                e.remainingTurns--;
                if (e.remainingTurns <= 0) manaTurnEffects.RemoveAt(i);
                else manaTurnEffects[i] = e;
            }
            Save();
        }

        // --- Persistence (optional ES3) ---
        private const string SaveKey = "run.manaTurnEffects";

        public void Save()
        {
            // Plug in Easy Save 3 if you’re using it; otherwise stub this out.
            // ES3.Save(SaveKey, manaTurnEffects);
        }

        public void Load()
        {
            // if (ES3.KeyExists(SaveKey)) manaTurnEffects = ES3.Load<List<ManaPerTurnEffect>>(SaveKey);
        }

        private void Awake()
        {
            // Make this survive map/scene loads if desired:
            // DontDestroyOnLoad(this.gameObject);
            Load();
        }
    }
}
