#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace ConjurerDice.EditorTools
{
    public class TileSnapperWindow : EditorWindow
    {
        private BoardGrid board;
        private int lane;
        private int index;
        private bool clampToBounds = true;

        [MenuItem("Tools/ConjurerDice/Snap to Tile...")]
        public static void Open()
        {
            var w = GetWindow<TileSnapperWindow>("Snap to Tile");
            w.minSize = new Vector2(320, 160);
            w.Show();
        }

        private void OnEnable()
        {
            if (!board) board = FindFirstObjectByType<BoardGrid>();
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("Board / Target Tile", EditorStyles.boldLabel);
            using (new EditorGUILayout.VerticalScope("box"))
            {
                board = (BoardGrid)EditorGUILayout.ObjectField("BoardGrid", board, typeof(BoardGrid), true);

                using (new EditorGUILayout.HorizontalScope())
                {
                    lane = EditorGUILayout.IntField("Lane", lane);
                    if (GUILayout.Button("−", GUILayout.Width(24))) lane--;
                    if (GUILayout.Button("+", GUILayout.Width(24))) lane++;
                }

                using (new EditorGUILayout.HorizontalScope())
                {
                    index = EditorGUILayout.IntField("Index", index);
                    if (GUILayout.Button("−", GUILayout.Width(24))) index--;
                    if (GUILayout.Button("+", GUILayout.Width(24))) index++;
                }

                clampToBounds = EditorGUILayout.ToggleLeft("Clamp to board bounds", clampToBounds);
            }

            EditorGUILayout.Space(6);

            using (new EditorGUI.DisabledScope(board == null))
            {
                if (GUILayout.Button("Snap Selected to Tile (Lane, Index)", GUILayout.Height(32)))
                {
                    SnapSelection();
                }

                using (new EditorGUILayout.HorizontalScope())
                {
                    if (GUILayout.Button("First Empty From Back"))
                    {
                        if (board != null)
                        {
                            var t = board.FirstEmptyFromBack(lane);
                            if (t.HasValue)
                            {
                                index = t.Value.index;
                                SnapSelection();
                            }
                            else Debug.LogWarning($"[TileSnapper] Lane {lane} is full.");
                        }
                    }
                    if (GUILayout.Button("First Empty From Front"))
                    {
                        if (board != null)
                        {
                            var t = board.FirstEmptyFromFront(lane);
                            if (t.HasValue)
                            {
                                index = t.Value.index;
                                SnapSelection();
                            }
                            else Debug.LogWarning($"[TileSnapper] Lane {lane} is full.");
                        }
                    }
                }
            }

            EditorGUILayout.HelpBox(
                "Select any Minion or Enemy in the Hierarchy/Scene, set a lane & index, then click Snap.",
                MessageType.Info);
        }

        private void SnapSelection()
        {
            if (board == null)
            {
                Debug.LogError("[TileSnapper] No BoardGrid assigned.");
                return;
            }

            var target = new TileRef(
                Mathf.Clamp(lane, 0, board.Lanes - 1),
                Mathf.Clamp(index, 0, board.LaneLength - 1));

            if (!board.IsInBounds(target))
            {
                if (clampToBounds)
                {
                    target.lane  = Mathf.Clamp(target.lane,  0, board.Lanes - 1);
                    target.index = Mathf.Clamp(target.index, 0, board.LaneLength - 1);
                }
                else
                {
                    Debug.LogError($"[TileSnapper] Tile out of bounds: L{target.lane}:{target.index}");
                    return;
                }
            }

            foreach (var obj in Selection.gameObjects)
            {
                if (obj == null) continue;
                SnapOne(obj, board, target);
            }
        }

        public static void SnapOne(GameObject go, BoardGrid board, TileRef target)
        {
            if (go == null || board == null) return;

            Undo.RegisterFullObjectHierarchyUndo(go, "Snap to Tile");

            // Prefer controller.Teleport(TileRef)
            var minion = go.GetComponentInParent<MinionController>();
            if (minion != null)
            {
                minion.Teleport(target);
                EditorUtility.SetDirty(minion);
                return;
            }

            var enemy = go.GetComponentInParent<EnemyController>();
            if (enemy != null)
            {
                enemy.Teleport(target);
                EditorUtility.SetDirty(enemy);
                return;
            }

            // Fallback: place transform at tile center (if board can provide it)
            var tile = board.GetTile(target); // assumes your BoardGrid exposes this; else replace with your accessor
            if (tile != null)
            {
                go.transform.position = tile.transform.position;
            }
            else
            {
                Debug.LogWarning($"[TileSnapper] BoardGrid.GetTile returned null for L{target.lane}:{target.index}. " +
                                 "Implement GetTile or add a placement helper.");
            }

            EditorUtility.SetDirty(go);
        }
    }
}
#endif
