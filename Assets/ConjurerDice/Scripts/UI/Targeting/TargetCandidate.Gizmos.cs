#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace ConjurerDice
{
    public partial class TargetCandidate
    {
        [SerializeField] private Color gizmos_Color = new Color(1f, 1f, 0f, 0.75f);
        [SerializeField] private float gizmos_Radius = 0.25f;

        private void OnDrawGizmosSelected()
        {
            Handles.color = gizmos_Color;
            Handles.SphereHandleCap(0, transform.position + Vector3.up * 0.1f,
                Quaternion.identity, gizmos_Radius, EventType.Repaint);

            // Small label with allegiance/lane:index
            Handles.Label(transform.position + Vector3.up * 0.2f,
                isAlly ? $"Ally  L{lane}:{index}" : $"Enemy L{lane}:{index}");
        }
    }
}
#endif