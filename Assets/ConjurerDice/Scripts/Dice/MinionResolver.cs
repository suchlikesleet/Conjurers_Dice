using UnityEngine;

namespace ConjurerDice
{
    public class MinionResolver : MonoBehaviour
    {
        [SerializeField] private BoardGrid board;

        public void Resolve(int lane, DiceFaceSO face)
        {
            if (board == null) board = FindFirstObjectByType<BoardGrid>();
            if (board == null || face == null || face.minion == null || face.minion.prefab == null) return;

            bool front = face.minion.spawnsAtFront;
            int index = front ? board.LaneLength - 1 : 0;
            var spawn = new TileRef(lane, index);

            // Simple occupancy check: if another ally already exactly here, fizzle
            if (IsTileOccupied(spawn))
            {
#if UNITY_EDITOR
                Debug.Log($"[MinionResolver] Lane {lane} {(front ? "front" : "back")} occupied → fizzle.");
#endif
                return;
            }

            var go = Instantiate(face.minion.prefab);
            var mc = go.GetComponent<MinionController>();
            if (mc == null) mc = go.AddComponent<MinionController>();
            mc.isAlly = true;
            mc.stats = face.minion.baseStats;
            mc.SpawnsAtFront = front;
            mc.Teleport(spawn);
#if UNITY_EDITOR
            Debug.Log($"[MinionResolver] Summoned {face.minion.displayName} at {spawn.lane}:{spawn.index}");
#endif
        }

        private bool IsTileOccupied(TileRef t)
        {
            var allies = FindObjectsByType<MinionController>(FindObjectsSortMode.None);
            foreach (var a in allies)
                if (a != null && a.tile.lane == t.lane && a.tile.index == t.index) return true;

            var enemies = FindObjectsByType<EnemyController>(FindObjectsSortMode.None);
            foreach (var e in enemies)
                if (e != null && e.tile.lane == t.lane && e.tile.index == t.index) return true;

            return false;
        }
    }
}