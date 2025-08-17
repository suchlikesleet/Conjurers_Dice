#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace ConjurerDice
{
    // Extends your existing BoardGrid without modifying it.
    public partial class BoardGrid : MonoBehaviour
    {
        [Header("Gizmos")]
        [SerializeField] private bool gizmos_DrawGrid = true;
        [SerializeField] private bool gizmos_DrawIndices = true;
        [SerializeField] private bool gizmos_DrawFrontBack = true;
        [SerializeField] private Color gizmos_LaneColor = new Color(0.25f, 0.8f, 1f, 0.5f);
        [SerializeField] private Color gizmos_TileColor = new Color(0.2f, 1f, 0.4f, 0.35f);
        [SerializeField] private Color gizmos_FrontColor = new Color(1f, 0.5f, 0.1f, 0.9f);
        [SerializeField] private Color gizmos_BackColor  = new Color(0.8f, 0.8f, 0.8f, 0.9f);
        [SerializeField] private float gizmos_TileSize = 1.0f;
        [SerializeField] private Vector3 gizmos_OriginOffset = Vector3.zero; // tweak to align with your board mesh

        private Vector3 TileCenter(int lane, int index)
        {
            // X = lane, Z = index (feel free to swap to fit your scene)
            return transform.TransformPoint(
                gizmos_OriginOffset + new Vector3(lane * gizmos_TileSize, 0f, index * gizmos_TileSize));
        }

        private void OnDrawGizmos()
        {
            if (!gizmos_DrawGrid) return;

            // lanes & tiles
            for (int ln = 0; ln < Lanes; ln++)
            {
                // lane bounds
                Handles.color = gizmos_LaneColor;
                var a = TileCenter(ln, 0)     + Vector3.left  * (gizmos_TileSize * 0.45f);
                var b = TileCenter(ln, 0)     + Vector3.right * (gizmos_TileSize * 0.45f);
                var c = TileCenter(ln, LaneLength - 1) + Vector3.right * (gizmos_TileSize * 0.45f);
                var d = TileCenter(ln, LaneLength - 1) + Vector3.left  * (gizmos_TileSize * 0.45f);
                Handles.DrawAAPolyLine(3f, a, d);
                Handles.DrawAAPolyLine(3f, b, c);

                // tiles
                Gizmos.color = gizmos_TileColor;
                for (int idx = 0; idx < LaneLength; idx++)
                {
                    var p = TileCenter(ln, idx);
                    Gizmos.DrawWireCube(p, new Vector3(gizmos_TileSize * 0.9f, 0.001f, gizmos_TileSize * 0.9f));
                    if (gizmos_DrawIndices)
                    {
                        Handles.color = Color.white;
                        Handles.Label(p + Vector3.up * 0.02f, $"L{ln}: {idx}");
                    }
                }

                if (gizmos_DrawFrontBack)
                {
                    // front/back markers (occupied tiles if helpers exist; else static 0 / last)
                    int back  = BackIndexForLane(ln);   // from BoardGrid.Helpers.cs
                    int front = FrontIndexForLane(ln);  // from BoardGrid.Helpers.cs

                    if (back >= 0)
                    {
                        Handles.color = gizmos_BackColor;
                        Handles.SphereHandleCap(0, TileCenter(ln, back) + Vector3.up * 0.05f,
                            Quaternion.identity, gizmos_TileSize * 0.25f, EventType.Repaint);
                        Handles.Label(TileCenter(ln, back) + Vector3.up * 0.12f, "Back");
                    }

                    if (front >= 0)
                    {
                        Handles.color = gizmos_FrontColor;
                        Handles.SphereHandleCap(0, TileCenter(ln, front) + Vector3.up * 0.05f,
                            Quaternion.identity, gizmos_TileSize * 0.25f, EventType.Repaint);
                        Handles.Label(TileCenter(ln, front) + Vector3.up * 0.12f, "Front");
                    }
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            // draw a little axis at origin to help orient
            var o = transform.TransformPoint(gizmos_OriginOffset);
            Handles.color = Color.red;   Handles.ArrowHandleCap(0, o, Quaternion.LookRotation(transform.right), 0.5f, EventType.Repaint);
            Handles.color = Color.blue;  Handles.ArrowHandleCap(0, o, Quaternion.LookRotation(transform.forward), 0.5f, EventType.Repaint);
        }
    }
}
#endif
