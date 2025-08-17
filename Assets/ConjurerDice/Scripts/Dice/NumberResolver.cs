using UnityEngine;

namespace ConjurerDice
{
    public class NumberResolver : MonoBehaviour
    {
        [SerializeField] private BoardGrid board;

        public void Resolve(int lane, DiceFaceSO face)
        {
            if (board == null) board = FindFirstObjectByType<BoardGrid>();
            if (board == null || face == null) return;

            // Find first ally minion in lane (closest to front)
            var minions = FindObjectsByType<MinionController>(FindObjectsSortMode.None);
            MinionController target = null;
            int bestIndex = -1;

            foreach (var m in minions)
            {
                if (m == null || !m.isAlly) continue;
                if (m.tile.lane != lane) continue;
                if (!board.IsInBounds(m.tile)) continue;
                if (m.tile.index > bestIndex) { bestIndex = m.tile.index; target = m; }
            }

            if (target == null)
            {
#if UNITY_EDITOR
                Debug.Log($"[NumberResolver] No ally in lane {lane} → wasted number.");
#endif
                return;
            }

            int movePoints = Mathf.Clamp(face.numberValue, 0, board.LaneLength);
            int tiles = MovementSystem.CalculateMovement(movePoints, target.stats);

            // naive move forward toward front (higher index)
            var newIndex = Mathf.Min(board.LaneLength - 1, target.tile.index + tiles);
            target.Teleport(new TileRef(lane, newIndex));
#if UNITY_EDITOR
            Debug.Log($"[NumberResolver] Moved {target.name} from {bestIndex} → {newIndex} (lane {lane})");
#endif
        }
    }
}