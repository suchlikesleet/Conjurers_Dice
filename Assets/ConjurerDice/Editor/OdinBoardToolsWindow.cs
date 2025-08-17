#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;

namespace ConjurerDice.EditorTools
{
    public class OdinBoardToolsWindow : OdinMenuEditorWindow
    {
        [MenuItem("Tools/ConjurerDice/Odin ▸ Board Tools")]
        private static void Open()
        {
            var w = GetWindow<OdinBoardToolsWindow>();
            w.titleContent = new GUIContent("Board Tools");
            w.minSize = new Vector2(420, 320);
            w.Show();
        }

        private SnapPage _snap;
        private SpawnPage _spawn;

        protected override void OnEnable()
        {
            base.OnEnable();
            _snap  = new SnapPage();
            _spawn = new SpawnPage();
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree(true);
            tree.DefaultMenuStyle = OdinMenuStyle.TreeViewStyle;
            tree.Add("Snap",  _snap);
            tree.Add("Spawn", _spawn);
            return tree;
        }

        // ---------------- SNAP ----------------
        [HideReferenceObjectPicker]
        public class SnapPage
        {
            [BoxGroup("Board & Target Tile"), Required, AssetSelector(Paths = "Assets")]
            [LabelText("BoardGrid")]
            public ConjurerDice.BoardGrid Board;

            [BoxGroup("Board & Target Tile"), LabelText("Lane"), ValueDropdown(nameof(LaneOptions))]
            public int Lane;

            [BoxGroup("Board & Target Tile"), LabelText("Index"), ValueDropdown(nameof(IndexOptions))]
            public int Index;

            [BoxGroup("Board & Target Tile")]
            [LabelText("Clamp to bounds")]
            public bool ClampToBounds = true;

            [ShowInInspector, ReadOnly, FoldoutGroup("Selection"), TableList(AlwaysExpanded = true)]
            private List<SelItem> SelectionPreview => BuildSelectionPreview();

            [System.Serializable]
            public class SelItem
            {
                [TableColumnWidth(220)] public GameObject Object;
                [TableColumnWidth(60)]  [LabelText("Lane"), ReadOnly]  public int Lane;
                [TableColumnWidth(60)]  [LabelText("Index"), ReadOnly] public int Index;
                [TableColumnWidth(80)]  [LabelText("Type"), ReadOnly]  public string Type;
            }

            [HorizontalGroup("Actions", 2, LabelWidth = 1)]
            [Button("◀ First Empty From Back", ButtonSizes.Medium)]
            [GUIColor(0.2f, 0.6f, 1f)]
            private void PickFirstEmptyBack()
            {
                if (!EnsureBoard()) return;
                var t = Board.FirstEmptyFromBack(Lane);
                if (t.HasValue) Index = t.Value.index;
                else EditorUtility.DisplayDialog("Lane Full", $"Lane {Lane} has no empty tiles (from back).", "OK");
            }

            [HorizontalGroup("Actions")]
            [Button("First Empty From Front ▶", ButtonSizes.Medium)]
            [GUIColor(0.2f, 0.6f, 1f)]
            private void PickFirstEmptyFront()
            {
                if (!EnsureBoard()) return;
                var t = Board.FirstEmptyFromFront(Lane);
                if (t.HasValue) Index = t.Value.index;
                else EditorUtility.DisplayDialog("Lane Full", $"Lane {Lane} has no empty tiles (from front).", "OK");
            }

            
            [InfoBox("@ValidationMessage()", InfoMessageType = InfoMessageType.None)]
            [HorizontalGroup("Do", 2, LabelWidth = 1)]
            [Button("Snap Selected", ButtonSizes.Large), GUIColor(0.1f, 0.8f, 0.3f)]
            private void SnapSelected()
            {
                if (!EnsureBoard()) return;
                var target = SafeTileRef();
                foreach (var go in Selection.gameObjects)
                    TileSnapCore.SnapOne(go, Board, target);
            }

            [HorizontalGroup("Do")]
            [Button("Focus Tile", ButtonSizes.Large)]
            private void FocusTile()
            {
                if (!EnsureBoard()) return;
                var tile = Board.GetTile(SafeTileRef());
                if (tile) SceneView.lastActiveSceneView?.Frame(new Bounds(tile.transform.position, Vector3.one), false);
            }

            // --- helpers ---
            private IEnumerable<int> LaneOptions()
            {
                if (!Board) yield break;
                for (int i = 0; i < Board.Lanes; i++) yield return i;
            }
            private IEnumerable<int> IndexOptions()
            {
                if (!Board) yield break;
                for (int i = 0; i < Board.LaneLength; i++) yield return i;
            }
            private bool EnsureBoard()
            {
                if (Board) return true;
                Board = Object.FindFirstObjectByType<ConjurerDice.BoardGrid>();
                if (!Board) EditorUtility.DisplayDialog("No BoardGrid", "Assign a BoardGrid in the field above.", "OK");
                return Board;
            }
            private ConjurerDice.TileRef SafeTileRef()
            {
                var lane  = Board ? Mathf.Clamp(Lane,  0, Board.Lanes - 1) : Lane;
                var index = Board ? Mathf.Clamp(Index, 0, Board.LaneLength - 1) : Index;
                var t = new ConjurerDice.TileRef(lane, index);

                if (Board && !Board.IsInBounds(t))
                {
                    if (ClampToBounds)
                    {
                        t.lane  = Mathf.Clamp(t.lane,  0, Board.Lanes - 1);
                        t.index = Mathf.Clamp(t.index, 0, Board.LaneLength - 1);
                    }
                }
                return t;
            }
            private string ValidationMessage()
            {
                if (!Board) return "Assign a BoardGrid to enable snapping.";
                var t = new ConjurerDice.TileRef(Lane, Index);
                if (!Board.IsInBounds(t)) return $"Tile L{Lane}:{Index} is OUT OF BOUNDS.";
                var occupied = Board.IsTileOccupied(t);
                return occupied ? $"Tile L{Lane}:{Index} is currently occupied." : $"Tile L{Lane}:{Index} is free.";
            }
            private List<SelItem> BuildSelectionPreview()
            {
                var list = new List<SelItem>();
                foreach (var go in Selection.gameObjects)
                {
                    if (!go) continue;
                    int lane = 0, idx = 0; string ty = "Other";
                    var tc = go.GetComponentInParent<ConjurerDice.TargetCandidate>();
                    if (tc) { lane = tc.lane; idx = tc.index; ty = tc.isAlly ? "Ally" : "Enemy"; }
                    else
                    {
                        var min = go.GetComponentInParent<ConjurerDice.MinionController>();
                        if (min) { lane = min.tile.lane; idx = min.tile.index; ty = "Ally"; }
                        var en = go.GetComponentInParent<ConjurerDice.EnemyController>();
                        if (en)  { lane = en.tile.lane;  idx = en.tile.index;  ty = "Enemy"; }
                    }
                    list.Add(new SelItem { Object = go, Lane = lane, Index = idx, Type = ty });
                }
                return list;
            }
        }

        // ---------------- SPAWN ----------------
        [HideReferenceObjectPicker]
        public class SpawnPage
        {
            [BoxGroup("Board & Target Tile"), Required, AssetSelector(Paths = "Assets")]
            [LabelText("BoardGrid")]
            public ConjurerDice.BoardGrid Board;

            [BoxGroup("Board & Target Tile"), LabelText("Lane"), ValueDropdown(nameof(LaneOptions))]
            public int Lane;

            [BoxGroup("Board & Target Tile"), LabelText("Index"), ValueDropdown(nameof(IndexOptions))]
            [Tooltip("If Use First-Empty buttons below, this will get overwritten.")]
            public int Index;

            [BoxGroup("Board & Target Tile")]
            [LabelText("Clamp to bounds")]
            public bool ClampToBounds = true;

            [FoldoutGroup("Prefabs"), LabelText("Ally Prefab")]  public GameObject AllyPrefab;
            [FoldoutGroup("Prefabs"), LabelText("Enemy Prefab")] public GameObject EnemyPrefab;

            [FoldoutGroup("Spawn Settings")]
            [LabelText("Count")] [MinValue(1)] public int Count = 1;
            [FoldoutGroup("Spawn Settings")]
            [LabelText("From Back (Ally) / From Front (Enemy)")]
            public bool UseSmartDefault = true;

            [ShowInInspector, ReadOnly, FoldoutGroup("Preview"), TableList(AlwaysExpanded = true)]
            private List<PreviewItem> LanePreview => BuildLanePreview();

            [System.Serializable]
            public class PreviewItem
            {
                [TableColumnWidth(60)] public int Index;
                [TableColumnWidth(100)][LabelText("Occupied?")] public bool Occupied;
                [TableColumnWidth(180)] public string By;
            }

            [HorizontalGroup("Q", 2, LabelWidth = 1)]
            [Button("◀ First Empty From Back", ButtonSizes.Medium)]
            [GUIColor(0.2f, 0.6f, 1f)]
            private void FirstEmptyBack()
            {
                if (!EnsureBoard()) return;
                var t = Board.FirstEmptyFromBack(Lane);
                if (t.HasValue) Index = t.Value.index;
                else EditorUtility.DisplayDialog("Lane Full", $"Lane {Lane} has no empty tiles (from back).", "OK");
            }

            [HorizontalGroup("Q")]
            [Button("First Empty From Front ▶", ButtonSizes.Medium)]
            [GUIColor(0.2f, 0.6f, 1f)]
            private void FirstEmptyFront()
            {
                if (!EnsureBoard()) return;
                var t = Board.FirstEmptyFromFront(Lane);
                if (t.HasValue) Index = t.Value.index;
                else EditorUtility.DisplayDialog("Lane Full", $"Lane {Lane} has no empty tiles (from front).", "OK");
            }

            
            [HorizontalGroup("DoA", 2, LabelWidth = 1)]
            [Button("Spawn Ally", ButtonSizes.Large), GUIColor(0.1f, 0.8f, 0.3f)]
            private void SpawnAlly()
            {
                if (!EnsureBoard()) return;
                if (!AllyPrefab) { EditorUtility.DisplayDialog("No Ally Prefab", "Assign Ally Prefab first.", "OK"); return; }
                SpawnBatch(AllyPrefab, isAlly: true);
            }

            [HorizontalGroup("DoA")]
            [Button("Spawn Enemy", ButtonSizes.Large), GUIColor(0.85f, 0.35f, 0.35f)]
            private void SpawnEnemy()
            {
                if (!EnsureBoard()) return;
                if (!EnemyPrefab) { EditorUtility.DisplayDialog("No Enemy Prefab", "Assign Enemy Prefab first.", "OK"); return; }
                SpawnBatch(EnemyPrefab, isAlly: false);
            }

            // --- helpers ---
            private IEnumerable<int> LaneOptions()
            {
                if (!Board) yield break;
                for (int i = 0; i < Board.Lanes; i++) yield return i;
            }
            private IEnumerable<int> IndexOptions()
            {
                if (!Board) yield break;
                for (int i = 0; i < Board.LaneLength; i++) yield return i;
            }
            private bool EnsureBoard()
            {
                if (Board) return true;
                Board = Object.FindFirstObjectByType<ConjurerDice.BoardGrid>();
                if (!Board) EditorUtility.DisplayDialog("No BoardGrid", "Assign a BoardGrid in the field above.", "OK");
                return Board;
            }
            private ConjurerDice.TileRef SafeTileRef()
            {
                var lane  = Board ? Mathf.Clamp(Lane,  0, Board.Lanes - 1) : Lane;
                var index = Board ? Mathf.Clamp(Index, 0, Board.LaneLength - 1) : Index;
                var t = new ConjurerDice.TileRef(lane, index);

                if (Board && !Board.IsInBounds(t))
                {
                    if (ClampToBounds)
                    {
                        t.lane  = Mathf.Clamp(t.lane,  0, Board.Lanes - 1);
                        t.index = Mathf.Clamp(t.index, 0, Board.LaneLength - 1);
                    }
                }
                return t;
            }
            private List<PreviewItem> BuildLanePreview()
            {
                var list = new List<PreviewItem>();
                if (!Board) return list;
                for (int i = 0; i < Board.LaneLength; i++)
                {
                    var t = new ConjurerDice.TileRef(Lane, i);
                    var occGo = Board.GetUnitAt(Lane, i);
                    list.Add(new PreviewItem
                    {
                        Index = i,
                        Occupied = occGo != null,
                        By = occGo ? occGo.name : "-"
                    });
                }
                return list;
            }

            private void SpawnBatch(GameObject prefab, bool isAlly)
            {
                var count = Mathf.Max(1, Count);
                int placed = 0;

                // Choose start index if smart default is on
                int cursor = Index;
                if (UseSmartDefault)
                {
                    var t = isAlly ? Board.FirstEmptyFromBack(Lane) : Board.FirstEmptyFromFront(Lane);
                    if (t.HasValue) cursor = t.Value.index;
                }

                for (int n = 0; n < count; n++)
                {
                    // Find next free tile starting from cursor (toward front for ally, toward back for enemy)
                    int step = isAlly ? +1 : -1;
                    int i = cursor;
                    bool placedThis = false;

                    while (i >= 0 && i < Board.LaneLength)
                    {
                        var tr = new ConjurerDice.TileRef(Lane, i);
                        if (!Board.IsTileOccupied(tr))
                        {
                            SpawnCore.SpawnOne(prefab, Board, tr, isAlly);
                            placed++;
                            placedThis = true;
                            i += step; // next search starts after this
                            break;
                        }
                        i += step;
                    }

                    if (!placedThis)
                    {
                        Debug.LogWarning($"[Spawn] No space left when placing #{n+1} on lane {Lane}.");
                        break;
                    }

                    cursor = Mathf.Clamp(i, 0, Board.LaneLength - 1);
                }

                if (placed > 0)
                {
                    // Ping lane tile used for the last spawn
                    var tile = Board.GetTile(new ConjurerDice.TileRef(Lane, cursor));
                    if (tile) EditorGUIUtility.PingObject(tile);
                    SceneView.lastActiveSceneView?.Repaint();
                }
            }
        }
    }

    // Shared snap core so Odin + non-Odin tools stay consistent
    internal static class TileSnapCore
    {
        public static void SnapOne(GameObject go, ConjurerDice.BoardGrid board, ConjurerDice.TileRef target)
        {
            if (go == null || board == null) return;

            Undo.RegisterFullObjectHierarchyUndo(go, "Snap to Tile");

            var minion = go.GetComponentInParent<ConjurerDice.MinionController>();
            if (minion != null)
            {
                minion.Teleport(target);
                EditorUtility.SetDirty(minion);
                return;
            }

            var enemy = go.GetComponentInParent<ConjurerDice.EnemyController>();
            if (enemy != null)
            {
                enemy.Teleport(target);
                EditorUtility.SetDirty(enemy);
                return;
            }

            var tile = board.GetTile(target);
            if (tile)
            {
                go.transform.position = tile.transform.position;
            }
            else
            {
                Debug.LogWarning($"[TileSnap] BoardGrid.GetTile returned null for L{target.lane}:{target.index}.");
            }

            EditorUtility.SetDirty(go);
        }
    }

    // Shared spawn core
    internal static class SpawnCore
    {
        public static GameObject SpawnOne(GameObject prefab, ConjurerDice.BoardGrid board, ConjurerDice.TileRef tile, bool isAlly)
        {
            if (!prefab || board == null) return null;

            var parent = board.transform; // place under board for tidiness
            var tileObj = board.GetTile(tile);
            Vector3 pos = tileObj ? tileObj.transform.position : parent.position;

            var go = (GameObject)PrefabUtility.InstantiatePrefab(prefab, parent);
            if (!go) go = Object.Instantiate(prefab, parent);
            go.transform.position = pos;

            // If controllers exist, use Teleport to register occupancy etc.
            var min = go.GetComponentInChildren<ConjurerDice.MinionController>();
            var en  = go.GetComponentInChildren<ConjurerDice.EnemyController>();

            if (isAlly && min != null)
            {
                min.Teleport(tile);
            }
            else if (!isAlly && en != null)
            {
                en.Teleport(tile);
            }
            else
            {
                // Fallback: set TargetCandidate coords if present
                var tc = go.GetComponentInChildren<ConjurerDice.TargetCandidate>();
                if (tc) { tc.lane = tile.lane; tc.index = tile.index; tc.isAlly = isAlly; }
            }

            // Ping in hierarchy
            EditorGUIUtility.PingObject(go);
            Undo.RegisterCreatedObjectUndo(go, "Spawn Unit");
            return go;
        }
    }
}
#endif
