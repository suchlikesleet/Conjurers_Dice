using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ConjurerDice
{
    // Keep EncounterRunner as partial so we add this without touching your other logic.
    public partial class EncounterRunner: MonoBehaviour
    {
        //[SerializeField] private BoardGrid board; // assign in inspector or auto-find

        private readonly List<EnemyController> _spawnedEnemies = new();

        // Call this at the start of an encounter BEFORE Player turn begins.
        // Example: StartEncounter(enc) => ConfigureBoard(enc); SpawnEnemies(enc);
        public void ConfigureBoard(EncounterSO enc)
        {
            if (!board) board = FindFirstObjectByType<BoardGrid>();
            if (!board || enc == null) return;

            // Just warn if different; do not assign read-only props
            if (board.Lanes != enc.lanes || board.LaneLength != enc.laneLength)
                Debug.LogWarning($"[Encounter] Board is {board.Lanes}x{board.LaneLength} but Encounter wants {enc.lanes}x{enc.laneLength}. Using board’s size.");
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(board);
#endif
        }

        public void SpawnEnemies(EncounterSO enc)
        {
            _spawnedEnemies.Clear();

            if (!board) board = FindFirstObjectByType<BoardGrid>();
            if (!board || enc == null || enc.enemies == null || enc.enemies.Length == 0) return;

            for (int i = 0; i < enc.enemies.Length; i++)
            {
                var enemySO = enc.enemies[i];
                if (enemySO == null) continue;

                // 1) Get prefab from EnemySO (supports common field names via reflection)
                var prefab = ExtractEnemyPrefab(enemySO);
                if (prefab == null)
                {
                    Debug.LogWarning($"[EncounterRunner] EnemySO '{enemySO.name}' has no prefab field I recognize. " +
                                     "Expected a field named 'prefab' or 'enemyPrefab'. Skipping.");
                    continue;
                }

                // 2) Pick lane: round-robin spread (0..Lanes-1). If you add per-enemy lane later, replace here.
                int lane = board.Lanes > 0 ? (i % board.Lanes) : 0;

                // 3) Decide tile: prefer first empty from FRONT (enemy side). Fallback = very front.
                var frontEmpty = board.FirstEmptyFromFront(lane);
                var spawnTile  = frontEmpty ?? new TileRef(lane, board.LaneLength - 1);

                // 4) Instantiate under this runner (or any container of your choice)
                var go = Instantiate(prefab, transform);
                var enemy = go.GetComponent<EnemyController>();
                if (enemy == null)
                {
                    Debug.LogWarning($"[EncounterRunner] Spawned prefab '{prefab.name}' has no EnemyController. Destroying.");
                    Destroy(go);
                    continue;
                }

                // 5) Place on board
                enemy.Teleport(spawnTile);

                // 6) Optional: seed simple numbers from EnemySO if present (e.g., baseDamage/attack)
                ApplyCommonStatsFromSO(enemySO, enemy);

                _spawnedEnemies.Add(enemy);
            }
        }

        // --- Helpers ----------------------------------------------------------

        private GameObject ExtractEnemyPrefab(Object enemySO)
        {
            if (enemySO == null) return null;

            // Try common field names: prefab, enemyPrefab, model, go
            var soType = enemySO.GetType();
            var fields = new[] { "prefab", "enemyPrefab", "model", "go" };

            foreach (var name in fields)
            {
                var f = soType.GetField(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (f != null && typeof(GameObject).IsAssignableFrom(f.FieldType))
                {
                    return (GameObject)f.GetValue(enemySO);
                }
            }

            return null;
        }

        private void ApplyCommonStatsFromSO(Object enemySO, EnemyController enemy)
        {
            if (enemySO == null || enemy == null) return;

            // Example: copy int baseDamage/attack if the EnemyController exposes a matching field
            var soType = enemySO.GetType();
            var dmgSrc = soType.GetField("baseDamage") ?? soType.GetField("attack") ?? soType.GetField("damage");
            if (dmgSrc != null && dmgSrc.FieldType == typeof(int))
            {
                int val = (int)dmgSrc.GetValue(enemySO);

                var eType = enemy.GetType();
                var dmgDst = eType.GetField("baseDamage") ?? eType.GetField("attack") ?? eType.GetField("damage");
                if (dmgDst != null && dmgDst.FieldType == typeof(int))
                    dmgDst.SetValue(enemy, val);
            }
        }
    }
}
