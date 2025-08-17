using UnityEngine;
using System.Collections.Generic;

namespace ConjurerDice
{
    public class AbilityResolver : MonoBehaviour
    {
        [Header("Services")]
        [SerializeField] private AbilityService abilityService;

        [Header("Targeting Channels")]
        [SerializeField] private TargetingRequestChannelSO targetingRequest;     // raise to start targeting UI
        [SerializeField] private TargetingResultEventChannelSO targetingResult;  // UI replies here

        // runtime
        private int _laneContext;
        private DiceFaceSO _pendingFace;
        private bool _waitingForTarget;

        private void Awake()
        {
            if (!abilityService) abilityService = FindFirstObjectByType<AbilityService>();
        }

        private void OnEnable()
        {
            if (targetingResult) targetingResult.OnResult += OnTargetingResult;
        }

        private void OnDisable()
        {
            if (targetingResult) targetingResult.OnResult -= OnTargetingResult;
        }

        /// <summary>
        /// Called by DiceResolver when a face of type Ability is rolled for a given lane.
        /// </summary>
        public void Resolve(int lane, DiceFaceSO face)
        {
            if (!face || !face.ability) return;

            // If this ability requires a target, request it and return.
            if (NeedsTarget(face.ability))
            {
                _laneContext = lane;
                _pendingFace = face;
                _waitingForTarget = true;

                targetingRequest?.Raise(face.ability, lane); // filter candidates to this lane if desired
#if UNITY_EDITOR
                Debug.Log($"[AbilityResolver] Requesting target for {face.ability.name} (lane {lane})");
#endif
                return;
            }

            // No target needed -> cast immediately
            var ctx = new AbilityCastContext
            {
                lane = lane,
                magnitude = face.ability.baseMagnitude,
                tile = new TileRef(lane, 0)
            };
            abilityService.TryCast(face.ability, ctx);
        }

        private bool NeedsTarget(AbilitySO ability)
        {
            switch (ability.targetKind)
            {
                case AbilitySO.TargetKind.UnitAlly:
                case AbilitySO.TargetKind.UnitEnemy:
                case AbilitySO.TargetKind.Tile:
                case AbilitySO.TargetKind.Multi:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// UI replies via TargetingController -> TargetingResultEventChannelSO.
        /// </summary>
        private void OnTargetingResult(List<GameObject> units, TileRef? tile)
        {
            if (!_waitingForTarget || _pendingFace == null) return;

            // Cancelled
            if (units == null && tile == null)
            {
#if UNITY_EDITOR
                Debug.Log("[AbilityResolver] Targeting cancelled.");
#endif
                _pendingFace = null;
                _waitingForTarget = false;
                return;
            }

            var ability = _pendingFace.ability;

            var ctx = new AbilityCastContext
            {
                lane = _laneContext,
                magnitude = ability.baseMagnitude,
                tile = tile ?? new TileRef(_laneContext, 0)
            };
            if (units != null && units.Count > 0)
                ctx.targets.AddRange(units);

            abilityService.TryCast(ability, ctx);

            // clear pending
            _pendingFace = null;
            _waitingForTarget = false;
        }
    }
}
