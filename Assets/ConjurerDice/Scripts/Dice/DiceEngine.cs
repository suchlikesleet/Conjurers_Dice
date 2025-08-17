using Obvious.Soap;
using UnityEngine;

namespace ConjurerDice
{
    public class DiceEngine : MonoBehaviour
    {
        [Header("Refs")]
        [SerializeField] private DiceRoller roller;
        [SerializeField] private DiceResolver resolver;
        [SerializeField] private ManaSystem mana;
        // DiceEngine.cs (additions)
        [SerializeField] private DicePoolManager pool;       // new
        [SerializeField] private ScriptableEventString onRollBlocked; // optional “not enough mana” toast

        public void RollFromPool()
        {
            // Only allow during player's turn
            if (!_playerPhase)
            {
                onRollBlocked?.Raise("You can only roll during your turn.");
                return;
            }

            // Ask the active pool for a die + face
            var (die, face) = pool != null ? pool.RollOne() : (null, null);

            if (face == null)
            {
                onRollBlocked?.Raise("No dice available to roll.");
                return;
            }

            // Pay mana (face.manaCost assumed on your DiceFaceSO)
            int cost = Mathf.Max(0, face.manaCost);
            if (!mana.SpendMana(cost))
            {
                onRollBlocked?.Raise("Not enough mana to roll!"); // or send a code; your UI can show “Not enough mana”
                return;
            }
            // Broadcast to HUD/FX and resolve the effect
            onDieRolled?.Raise(face);
            resolver.Resolve(_selectedLane, face);
        }
        
        [Header("Events")]
        [SerializeField] private TurnPhaseEventChannelSO onPhaseChanged;
        [SerializeField] private LaneEventChannelSO onLaneSelected;
        [SerializeField] private DiceFaceEventChannelSO onDieRolled;

        private int _selectedLane = 0;
        private bool _playerPhase = false;

        private void OnEnable()
        {
            if (onLaneSelected != null) onLaneSelected.OnRaised += SetLane;
            if (onPhaseChanged != null) onPhaseChanged.OnRaised += HandlePhase;
        }
        private void OnDisable()
        {
            if (onLaneSelected != null) onLaneSelected.OnRaised -= SetLane;
            if (onPhaseChanged != null) onPhaseChanged.OnRaised -= HandlePhase;
        }

        private void HandlePhase(TurnPhase p) => _playerPhase = (p == TurnPhase.Player);
        private void SetLane(int lane) => _selectedLane = lane;

        /// <summary>Call this from UI Button with a DiceSO ref.</summary>
        public void RollSelected(DiceSO dice)
        {
            if (!_playerPhase || dice == null) return;

            var face = roller.Roll(dice);
            if (face == null) return;

            // Spend mana before resolving
            int cost = Mathf.Max(0, face.manaCost);
            if (!mana.SpendMana(cost))
            {
#if UNITY_EDITOR
                Debug.Log("[DiceEngine] Not enough mana.");
#endif
                return;
            }

            onDieRolled?.Raise(face);
            resolver.Resolve(_selectedLane, face);
        }
    }
}