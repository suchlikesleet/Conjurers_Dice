// Scripts/Simulation/Abilities/TargetingRules.cs
using UnityEngine;
using System.Collections.Generic;

namespace ConjurerDice
{
    public enum TargetInvalidReason
    {
        None,
        WrongAllegiance,
        WrongLane,
        OutOfRange,
        Blocked,
        MaxTargetsReached
    }

    public struct TargetValidation
    {
        public bool valid;
        public TargetInvalidReason reason;
        public List<GameObject> affected; // includes the main target for AoE
    }

    public class TargetingRules : MonoBehaviour
    {
        [SerializeField] private BoardGrid board;

        private void Awake()
        {
            if (!board) board = FindFirstObjectByType<BoardGrid>();
        }

        public TargetValidation ValidateUnit(AbilitySO ability, int laneContext, GameObject unitGO)
        {
            var res = new TargetValidation { valid = false, reason = TargetInvalidReason.None, affected = new List<GameObject>() };
            if (!ability || !unitGO) return res;

            var cand = unitGO.GetComponentInParent<TargetCandidate>();
            if (!cand) return res;

            // Allegiance
            bool needAlly  = ability.targetKind == AbilitySO.TargetKind.UnitAlly;
            bool needEnemy = ability.targetKind == AbilitySO.TargetKind.UnitEnemy;
            if (needAlly && !cand.isAlly)  { res.reason = TargetInvalidReason.WrongAllegiance; return res; }
            if (needEnemy &&  cand.isAlly) { res.reason = TargetInvalidReason.WrongAllegiance; return res; }

            // Lane
            if (ability.requireSameLane && cand.lane != laneContext)
            { res.reason = TargetInvalidReason.WrongLane; return res; }

            // Range (lane index distance)
            int dist = Mathf.Abs(cand.index - board.FrontIndexForLane(laneContext)); // or from caster index if you track it
            // If you prefer from back, replace with your own distance function.
            if (dist < ability.minRange || dist > ability.maxRange)
            { res.reason = TargetInvalidReason.OutOfRange; return res; }

            // AoE shape collection
            res.affected = CollectAffected(ability, cand.lane, cand.index, unitGO);
            res.valid = true;
            return res;
        }

        public TargetValidation ValidateTile(AbilitySO ability, int laneContext, TileRef tile)
        {
            var res = new TargetValidation { valid = false, reason = TargetInvalidReason.None, affected = new List<GameObject>() };

            if (ability.requireSameLane && tile.lane != laneContext)
            { res.reason = TargetInvalidReason.WrongLane; return res; }

            int dist = Mathf.Abs(tile.index - board.FrontIndexForLane(laneContext));
            if (dist < ability.minRange || dist > ability.maxRange)
            { res.reason = TargetInvalidReason.OutOfRange; return res; }

            res.affected = CollectAffected(ability, tile.lane, tile.index, null);
            res.valid = true;
            return res;
        }

        // --- helpers ---
        private List<GameObject> CollectAffected(AbilitySO ability, int lane, int index, GameObject primary)
        {
            var list = new List<GameObject>();
            if (primary) list.Add(primary);

            switch (ability.shape)
            {
                case AbilitySO.Shape.Single:
                    break;

                case AbilitySO.Shape.Line:
                {
                    // march toward front and collect until a blocker (if enabled)
                    for (int i = index + 1; i < board.LaneLength; i++)
                    {
                        var u = UnitOn(lane, i);
                        if (u) list.Add(u);
                        if (ability.lineBlocksAtFirstUnit && u) break;
                    }
                    break;
                }

                case AbilitySO.Shape.Cross:
                {
                    // up/down along lane and adjacent lanes same index
                    for (int r = 1; r <= ability.shapeRadius; r++)
                    {
                        TryAdd(lane, index + r); // forward
                        TryAdd(lane, index - r); // back
                    }
                    TryAdd(lane - 1, index);
                    TryAdd(lane + 1, index);
                    break;
                }

                case AbilitySO.Shape.ConeForward:
                {
                    // narrow fan toward front, widening by radius
                    for (int r = 1; r <= ability.shapeRadius; r++)
                    {
                        int idx = index + r;
                        for (int dl = -r; dl <= r; dl++)
                            TryAdd(lane + dl, idx);
                    }
                    break;
                }

                case AbilitySO.Shape.LaneAoE:
                {
                    for (int i = Mathf.Max(0, index - ability.shapeRadius); i <= Mathf.Min(board.LaneLength - 1, index + ability.shapeRadius); i++)
                        TryAdd(lane, i);
                    break;
                }
            }

            return list;

            void TryAdd(int ln, int idx)
            {
                if (!board.IsInBounds(new TileRef(ln, idx))) return;
                var u = UnitOn(ln, idx);
                if (u) list.Add(u);
            }
        }

        private GameObject UnitOn(int lane, int index)
        {
            // naive scan – optimize later with a board occupancy map
            foreach (var e in FindObjectsByType<EnemyController>(FindObjectsSortMode.None))
                if (e && e.tile.lane == lane && e.tile.index == index) return e.gameObject;
            foreach (var a in FindObjectsByType<MinionController>(FindObjectsSortMode.None))
                if (a && a.tile.lane == lane && a.tile.index == index) return a.gameObject;
            return null;
        }
    }
}
