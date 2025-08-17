// Assets/ConjurerDice/Scripts/Simulation/Board/BoardGrid.Helpers.cs
using UnityEngine;
using System.Collections.Generic;

namespace ConjurerDice
{
    // Extends your existing BoardGrid without modifying it.
    public partial class BoardGrid
    {
        /// Return the unit (ally or enemy) occupying a tile, or null.
        public GameObject GetUnitAt(int lane, int index)
        {
            foreach (var e in FindObjectsByType<EnemyController>(FindObjectsSortMode.None))
                if (e && e.tile.lane == lane && e.tile.index == index) return e.gameObject;

            foreach (var a in FindObjectsByType<MinionController>(FindObjectsSortMode.None))
                if (a && a.tile.lane == lane && a.tile.index == index) return a.gameObject;

            return null;
        }

        public bool IsTileOccupied(TileRef t) => GetUnitAt(t.lane, t.index) != null;

        /// Iterate all units in a lane (optionally filter).
        public IEnumerable<GameObject> UnitsInLane(int lane, bool includeAllies = true, bool includeEnemies = true)
        {
            if (includeEnemies)
                foreach (var e in FindObjectsByType<EnemyController>(FindObjectsSortMode.None))
                    if (e && e.tile.lane == lane) yield return e.gameObject;

            if (includeAllies)
                foreach (var a in FindObjectsByType<MinionController>(FindObjectsSortMode.None))
                    if (a && a.tile.lane == lane) yield return a.gameObject;
        }

        /// Highest occupied index in lane (front). -1 if empty.
        public int FrontIndexForLane(int lane)
        {
            int best = -1;
            foreach (var go in UnitsInLane(lane))
            {
                var tc = go.GetComponentInParent<TargetCandidate>();
                if (tc && tc.index > best) best = tc.index;
            }
            return best;
        }

        /// Lowest occupied index in lane (back). -1 if empty.
        public int BackIndexForLane(int lane)
        {
            int best = int.MaxValue; bool found = false;
            foreach (var go in UnitsInLane(lane))
            {
                var tc = go.GetComponentInParent<TargetCandidate>();
                if (!tc) continue;
                found = true;
                if (tc.index < best) best = tc.index;
            }
            return found ? best : -1;
        }

        /// First empty tile from the back; null if full.
        public TileRef? FirstEmptyFromBack(int lane)
        {
            for (int i = 0; i < LaneLength; i++)
                if (!IsTileOccupied(new TileRef(lane, i))) return new TileRef(lane, i);
            return null;
        }

        /// First empty tile from the front; null if full.
        public TileRef? FirstEmptyFromFront(int lane)
        {
            for (int i = LaneLength - 1; i >= 0; i--)
                if (!IsTileOccupied(new TileRef(lane, i))) return new TileRef(lane, i);
            return null;
        }
    }
}
