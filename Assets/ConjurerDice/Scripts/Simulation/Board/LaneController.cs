using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace ConjurerDice
{
    public class LaneController : MonoBehaviour
    {
        [SerializeField] private int laneIndex;
        public int LaneIndex => laneIndex;

        // --- Queries (scene-scan MVP; replace with occupancy map later) ---
        public IEnumerable<GameObject> Allies()
        {
            foreach (var m in FindObjectsByType<MinionController>(FindObjectsSortMode.None))
                if (m && m.tile.lane == laneIndex) yield return m.gameObject;
        }

        public IEnumerable<GameObject> Enemies()
        {
            foreach (var e in FindObjectsByType<EnemyController>(FindObjectsSortMode.None))
                if (e && e.tile.lane == laneIndex) yield return e.gameObject;
        }

        public IEnumerable<GameObject> AllUnits(bool includeAllies = true, bool includeEnemies = true)
        {
            if (includeAllies)  foreach (var a in Allies())  yield return a;
            if (includeEnemies) foreach (var e in Enemies()) yield return e;
        }

        // Front = highest index; Back = lowest index
        public GameObject GetFrontMostAllyGO()
            => Allies().OrderBy(go => go.GetComponent<MinionController>().tile.index).LastOrDefault();

        public GameObject GetFrontMostEnemyGO()
            => Enemies().OrderBy(go => go.GetComponent<EnemyController>().tile.index).LastOrDefault();

        public int? FrontIndexOccupied()
        {
            var all = AllUnits(true, true)
                .Select(go => go.GetComponentInParent<TargetCandidate>())
                .Where(tc => tc != null && tc.lane == laneIndex)
                .Select(tc => tc.index);
            return all.Any() ? all.Max() : (int?)null;
        }

        public int? BackIndexOccupied()
        {
            var all = AllUnits(true, true)
                .Select(go => go.GetComponentInParent<TargetCandidate>())
                .Where(tc => tc != null && tc.lane == laneIndex)
                .Select(tc => tc.index);
            return all.Any() ? all.Min() : (int?)null;
        }

        public TileRef? FirstEmptyFromBack(int laneLength)
        {
            for (int i = 0; i < laneLength; i++)
                if (!IsOccupied(i)) return new TileRef(laneIndex, i);
            return null;
        }

        public TileRef? FirstEmptyFromFront(int laneLength)
        {
            for (int i = laneLength - 1; i >= 0; i--)
                if (!IsOccupied(i)) return new TileRef(laneIndex, i);
            return null;
        }

        public bool IsOccupied(int index)
        {
            foreach (var go in AllUnits(true, true))
            {
                var tc = go.GetComponentInParent<TargetCandidate>();
                if (tc != null && tc.lane == laneIndex && tc.index == index) return true;
            }
            return false;
        }
    }
}
