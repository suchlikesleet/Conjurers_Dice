#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;

namespace ConjurerDice.EditorTools
{
    public class OdinTileSnapperWindow : OdinEditorWindow
    {
        [MenuItem("Tools/ConjurerDice/Odin ▸ Snap to Tile...")]
        private static void Open()
        {
            var w = GetWindow<OdinTileSnapperWindow>();
            w.titleContent = new GUIContent("Snap to Tile (Odin)");
            w.minSize = new Vector2(360, 280);
            w.Show();
        }

        // --- Inputs ---
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
            [TableColumnWidth(180)]
            public GameObject Object;

            [TableColumnWidth(60)]
            [LabelText("Lane"), ReadOnly] public int Lane;
            [TableColumnWidth(60)]
            [LabelText("Index"), ReadOnly] public int Index;

            [TableColumnWidth(100)]
            [LabelText("Type"), ReadOnly] public string Type;
        }

        // Inline helpers row
        [HorizontalGroup("Actions", 2, LabelWidth = 1)]
        [Button("◀ First Empty From Back", ButtonSizes.Medium)]
        [GUIColor(0.2f, 0.6f, 1f)]
        private void PickFirstEmptyBack()
        {
            if (!EnsureBoard()) return;
            var t = Board.FirstEmptyFromBack(Lane);
            if ( t.HasValue) Index = t.Value.index;
            else EditorUtility.DisplayDialog("Lane Full", $"Lane {Lane} has no empty tiles from back.", "OK");
        }

        [HorizontalGroup("Actions")]
        [Button("First Empty From Front ▶", ButtonSizes.Medium)]
        [GUIColor(0.2f, 0.6f, 1f)]
        private void PickFirstEmptyFront()
        {
            if (!EnsureBoard()) return;
            var t = Board.FirstEmptyFromFront(Lane);
            if ( t.HasValue) Index = t.Value.index;
            else EditorUtility.DisplayDialog("Lane Full", $"Lane {Lane} has no empty tiles from front.", "OK");
        }

        //[Space(6)]
        [InfoBox("@ValidationMessage()", InfoMessageType = InfoMessageType.None)]
        [HorizontalGroup("Do", 2, LabelWidth = 1)]
        [Button("Snap Selected", ButtonSizes.Large), GUIColor(0.1f, 0.8f, 0.3f)]
        private void SnapSelected()
        {
            if (!EnsureBoard()) return;
            var target = SafeTileRef();
            foreach (var go in Selection.gameObjects)
                TileSnapperWindow.SnapOne(go, Board, target);
        }

        [HorizontalGroup("Do")]
        [Button("Focus Tile", ButtonSizes.Large)]
        private void FocusTile()
        {
            if (!EnsureBoard()) return;
            var tile = Board.GetTile(SafeTileRef());
            if (tile) SceneView.lastActiveSceneView.Frame(new Bounds(tile.transform.position, Vector3.one), false);
        }

        // --- ValueDropdown sources ---
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

        // --- Helpers ---
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

            if (!Board) return t;
            if (!Board.IsInBounds(t))
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
                int lane = 0, idx = 0;
                string ty = "Other";

                var tc = go.GetComponentInParent<ConjurerDice.TargetCandidate>();
                if (tc)
                {
                    lane = tc.lane; idx = tc.index; ty = tc.isAlly ? "Ally" : "Enemy";
                }
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
}
#endif
