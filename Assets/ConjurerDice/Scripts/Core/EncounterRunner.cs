using UnityEngine;
using System.Collections.Generic;

namespace ConjurerDice
{
    public partial class EncounterRunner : MonoBehaviour
    {
        [Header("Refs")]
        [SerializeField] private BoardGrid board;
        [SerializeField] private HazardSystem hazardSystem;
        [SerializeField] private StatusSystem statusSystem;
        [SerializeField] private AbilityQueue abilityQueue;
        [SerializeField] private BalanceConfigSO chapterBalance;
        [SerializeField] private ManaSystem mana;
        [SerializeField] private RunEffectsService runEffects;
        [SerializeField] private DicePoolManager dicePool;
        
        
        [SerializeField] private bool applyStartRegenOnTurn1 = false;
        private bool _isFirstTurn = true;
        
        // Encounter-scoped temporary buffs (cleared at encounter end)
        [System.Serializable]
        public struct ManaBuff { public int amount; public int remainingTurns; }
        private readonly List<ManaBuff> _encounterBuffs = new();
        
        [SerializeField] private TurnController turnController;

        [Header("Events")]
        [SerializeField] private TurnPhaseEventChannelSO onPhaseChanged;
        [SerializeField] private EncounterResultEventChannelSO onEncounterCompleted;
        [SerializeField] private DiceLoadoutConfirmedEventSO onLoadoutConfirmed;
        [SerializeField] private EnemyPhaseCoordinator enemyCoordinator;

        // Runtime state
        private readonly List<IEnemyActor> _enemies = new();
        private readonly List<MinionController> _allies = new();
        private EncounterSO _activeEncounter;
        public EncounterSO pendingEncounterSO; //maybe call from dice or other scripts

        private void OnEnable()
        {
            if (enemyCoordinator == null) enemyCoordinator = FindFirstObjectByType<EnemyPhaseCoordinator>();
            if (onPhaseChanged != null) onPhaseChanged.OnRaised += HandlePhase;
            if (onLoadoutConfirmed != null) onLoadoutConfirmed.OnRaised += HandleLoadoutConfirmed;
            
        }

        private void OnDisable()
        {
            if (onPhaseChanged != null) onPhaseChanged.OnRaised -= HandlePhase;
            if (onLoadoutConfirmed != null) onLoadoutConfirmed.OnRaised -= HandleLoadoutConfirmed;
        }

        // --- Boot/Build/Start ---
        public void StartEncounter(EncounterSO enc)
        {
            _activeEncounter = enc;
            
            if (mana == null) mana = FindFirstObjectByType<ManaSystem>();
            if (runEffects == null) runEffects = FindFirstObjectByType<RunEffectsService>();

            // Reset mana at encounter start to chapter baseline (NOT previous encounter’s current)
            int startMana = chapterBalance.startingMana + chapterBalance.relicBonusStart;
            mana.SetMana(startMana);

            _encounterBuffs.Clear(); // encounter-only buffs do NOT carry over
            BuildBoard(enc);
            SpawnEnemies(enc);
            RegisterAlliesInScene(); // If allies are spawned via dice later, this may be empty initially.

            // Kick the turn engine
            if (turnController == null) turnController = FindFirstObjectByType<TurnController>();
            turnController.BeginEncounter();
        }

        private void BuildBoard(EncounterSO enc)
        {
            if (board == null) board = FindFirstObjectByType<BoardGrid>();
            // TODO: size/visuals; for now we assume BoardGrid is already sized via BoardConfigSO
            if (hazardSystem == null) hazardSystem = FindFirstObjectByType<HazardSystem>();

            if (enc != null && enc.hazards != null)
            {
                // Optional: seed some hazards at fixed tiles if your data supports it
                // (You can extend EncounterSO later with starting hazard placements)
            }
        }

        
        private void RegisterAlliesInScene()
        {
            _allies.Clear();
            _allies.AddRange(FindObjectsByType<MinionController>(FindObjectsSortMode.None));
        }

        // --- Turn Phase reactions ---
        private void HandlePhase(TurnPhase phase)
        {
            switch (phase)
            {
                case TurnPhase.Start:
                    OnStartPhase();
                    break;
                case TurnPhase.Player:
                    OnPlayerPhase();
                    break;
                case TurnPhase.Enemy:
                    OnEnemyPhase(); // executed quickly; TurnController yields before advancing
                    break;
                case TurnPhase.End:
                    OnEndPhase();
                    break;
            }
        }

        private void OnStartPhase()
        {
            //UpdateMana
            UpdateMana();
            
            // Regen/status start-ticks
            statusSystem?.TickStartPhase();

            // Hazard start ticks if any type uses them
            hazardSystem?.TickStartPhase();
            
            // Now decay both sources by 1 turn (they applied this turn already)
            DecayEncounterBuffsOneTurn();
            runEffects?.DecayOneTurn();

            // After setup, allow player input
            SetPhasePlayer();
        }

        private void UpdateMana()
        {
            int chapterBase = chapterBalance.manaPerTurn + chapterBalance.relicBonusPerTurn;
            int runDelta    = runEffects != null ? runEffects.GetActiveManaPerTurnDelta() : 0;
            int encDelta    = SumEncounterBuffs(); // buffs/curses inside this encounter
            if (!_isFirstTurn || applyStartRegenOnTurn1)
            {
                mana?.GainMana(chapterBase + runDelta + encDelta);
            }
            _isFirstTurn = false;

        }

        private void OnPlayerPhase()
        {
            // The player will take actions via UI.
            // When done, UI should call turnController.EndPlayerPhase();
        }

        private void OnEnemyPhase()
        {
            // Resolve queued abilities if you let enemies push to abilityQueue
            abilityQueue?.ResolveAll();
            
            //Old method 
            /*foreach (var enemy in _enemies)
            {
                if (enemy == null) continue;
                enemy.TickEnemyTurn(board);
            }*/
            
            //Add a serialized field and kick off the coordinator inside your phase handler.
            if (enemyCoordinator == null) enemyCoordinator = FindFirstObjectByType<EnemyPhaseCoordinator>();
            enemyCoordinator?.BeginEnemyPhase();
            
        }

        private void OnEndPhase()
        {
            // End-of-turn ticks
            statusSystem?.TickEndPhase();
            hazardSystem?.TickEndPhase();

            // Check win/lose
            if (IsVictorious())
            {
                CompleteEncounter(true);
            }
            else if (IsDefeated())
            {
                CompleteEncounter(false);
            }
        }

        private void SetPhasePlayer()
        {
            // Let the TurnController drive the actual phase signal:
            // We just ask it to set Player phase.
            if (turnController != null) turnController.SetPhase(TurnPhase.Player);
        }

        private bool IsVictorious()
        {
            // Win if all enemies dead/removed
            foreach (var e in _enemies)
            {
                if (e != null && e.IsAlive) return false;
            }
            return true;
        }

        private bool IsDefeated()
        {
            // Simple rule: no allies on board (you can replace with hero/base HP check)
            foreach (var a in _allies)
            {
                if (a != null) return false;
            }
            return false; // default to false for now
        }
        
        public void AddTempEncounterManaBuff(int amountPerTurn, int turns)
        {
            if (turns <= 0 || amountPerTurn == 0) return;
            _encounterBuffs.Add(new ManaBuff { amount = amountPerTurn, remainingTurns = turns });
            Debug.Log($"[EncounterRunner] Encounter buff: {amountPerTurn}/turn for {turns} turns");
        }

// NEW: helper sums encounter buffs still active
        private int SumEncounterBuffs()
        {
            int s = 0;
            foreach (var b in _encounterBuffs) s += b.amount;
            return s;
        }

        private void DecayEncounterBuffsOneTurn()
        {
            for (int i = _encounterBuffs.Count - 1; i >= 0; i--)
            {
                var b = _encounterBuffs[i];
                b.remainingTurns--;
                if (b.remainingTurns <= 0) _encounterBuffs.RemoveAt(i);
                else _encounterBuffs[i] = b;
            }
        }
        
        private void HandleLoadoutConfirmed()
        {
            // now safe to start encounter with chosen dice
            dicePool.BuildEncounterPool();
            StartEncounter(pendingEncounterSO);
        }

        // On encounter end: DO NOT clear runEffects (they carry over).
        private void CompleteEncounter(bool win)
        {
            onEncounterCompleted?.Raise(new EncounterResult { Win = win });
#if UNITY_EDITOR
            Debug.Log($"[EncounterRunner] Encounter complete. Win={win}");
#endif
        }
    }
}
