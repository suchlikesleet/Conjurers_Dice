using UnityEngine;

namespace ConjurerDice
{
    /// Attach to units/tiles so Targeting can read lane/index & show highlights.
    public partial class TargetCandidate : MonoBehaviour
    {
        [Header("Board coords (runtime)")]
        public int lane = -1;
        public int index = 0;   // << added

        [Header("Flags")]
        public bool isAlly = false;

        [Header("Visuals")]
        [SerializeField] private GameObject highlightRoot;
        [SerializeField] private GameObject reticleRoot;

        // --- Optional: keep lane/index in sync from controllers ---
        void LateUpdate()
        {
            // If this is a unit, mirror its tile coords every frame
            var ally = GetComponentInParent<MinionController>();
            if (ally != null)
            {
                lane  = ally.tile.lane;
                index = ally.tile.index;
                isAlly = true;
                return;
            }

            var enemy = GetComponentInParent<EnemyController>();
            if (enemy != null)
            {
                lane  = enemy.tile.lane;
                index = enemy.tile.index;
                isAlly = false;
                return;
            }

            // For tile prefabs, lane/index can be set by the spawner/grid.
        }

        public void SetHighlight(bool on) { if (highlightRoot) highlightRoot.SetActive(on); }
        public void SetReticle(bool on)   { if (reticleRoot)   reticleRoot.SetActive(on); }
    }
}