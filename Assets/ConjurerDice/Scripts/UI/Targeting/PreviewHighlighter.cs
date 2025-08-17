// Scripts/UI/Targeting/PreviewHighlighter.cs
using UnityEngine;
using System.Collections.Generic;

namespace ConjurerDice
{
    public class PreviewHighlighter : MonoBehaviour
    {
        [SerializeField] private Color validColor = Color.cyan;
        [SerializeField] private Color invalidColor = Color.red;

        private readonly List<TargetCandidate> _last = new();

        public void ShowPreview(List<GameObject> affected, bool valid)
        {
            Clear();
            if (affected == null) return;

            foreach (var go in affected)
            {
                if (!go) continue;
                var tc = go.GetComponentInParent<TargetCandidate>();
                if (!tc) continue;
                tc.SetHighlight(true);
                // optional: tint by valid/invalid
                // (If your highlightRoot has a renderer, set color here)
                _last.Add(tc);
            }
        }

        public void Clear()
        {
            foreach (var c in _last) if (c) { c.SetHighlight(false); c.SetReticle(false); }
            _last.Clear();
        }
    }
}