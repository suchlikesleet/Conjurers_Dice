using UnityEngine;
using System.Collections;
using UnityEngine.Events;

namespace ConjurerDice
{
    public class TurnController : MonoBehaviour
    {
        [Header("Events")]
        [SerializeField] private IntEventChannelSO onTurnStarted;
        [SerializeField] private IntEventChannelSO onTurnEnded;
        [SerializeField] private TurnPhaseEventChannelSO onPhaseChanged;
        [SerializeField] private VoidEventChannelSO onEnemyPhaseAllDone;

        [Header("Tick Durations")]
        [Tooltip("Seconds to wait between automatic phase transitions (enemy & end).")]
        [SerializeField] private float enemyPhaseDelay = 0.3f;
        [SerializeField] private float endPhaseDelay = 0.2f;

        public int TurnIndex { get; private set; } = 0;
        public TurnPhase CurrentPhase { get; private set; } = TurnPhase.Start;

        public void BeginEncounter()
        {
            TurnIndex = 1;
            SetPhase(TurnPhase.Start);
            onTurnStarted?.Raise(TurnIndex);
        }

        /// <summary>Call this from UI / input when the player commits their actions.</summary>
        public void EndPlayerPhase()
        {
            if (CurrentPhase != TurnPhase.Player) return;
            SetPhase(TurnPhase.Enemy);
            StartCoroutine(RunEnemyPhaseThenEnd());
        }

        // --- Internals ---
        public void SetPhase(TurnPhase next)
        {
            CurrentPhase = next;
            onPhaseChanged?.Raise(CurrentPhase);
#if UNITY_EDITOR
            Debug.Log($"[TurnController] Phase -> {CurrentPhase} (Turn {TurnIndex})");
#endif
        }

        private IEnumerator RunEnemyPhaseThenEnd()
        {
            // Enemy phase has just been entered via SetPhase(TurnPhase.Enemy)
            // Wait until EnemyPhaseCoordinator signals completion.
            bool finished = false;
            UnityAction handler = () => finished = true;
            if (onEnemyPhaseAllDone != null) onEnemyPhaseAllDone.OnRaised += handler;

            // Optional tiny frame so listeners can kick off
            yield return null;

            // Safety fallback to avoid hard locks in early dev
            float safetyTimer = 10f; // seconds
            while (!finished && safetyTimer > 0f)
            {
                safetyTimer -= Time.deltaTime;
                yield return null;
            }

            if (onEnemyPhaseAllDone != null)
            {
                onEnemyPhaseAllDone.OnRaised -= handler;
                
            }

            // Proceed to End phase → next turn
            SetPhase(TurnPhase.End);
            yield return new WaitForSeconds(endPhaseDelay);

            onTurnEnded?.Raise(TurnIndex);
            TurnIndex++;
            onTurnStarted?.Raise(TurnIndex);
            SetPhase(TurnPhase.Start);
        }
    }
}