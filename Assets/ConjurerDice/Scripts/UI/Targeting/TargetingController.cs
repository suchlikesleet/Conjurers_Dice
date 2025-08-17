// Scripts/UI/Targeting/TargetingController.cs  (V2)
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace ConjurerDice
{
    public class TargetingController : MonoBehaviour
    {
        [Header("Events")]
        [SerializeField] private TargetingRequestChannelSO targetingRequest;
        [SerializeField] private TargetingResultEventChannelSO targetingResult;
        [SerializeField] private TargetingFeedbackEventChannelSO feedback;

        [Header("Input Bus")]
        [SerializeField] private InputBusTargetingSO inputBus;

        [Header("Refs")]
        [SerializeField] private TargetingRules rules;
        [SerializeField] private PreviewHighlighter preview;
        [SerializeField] private BoardGrid board;
        [SerializeField] private CanvasGroup prompt;

        private AbilitySO _ability;
        private int? _laneFilter;
        private readonly List<TargetCandidate> _eligible = new();
        private readonly List<GameObject> _selected = new();
        private Vector2 _pointer;

        private void Awake()
        {
            if (!rules)   rules   = FindFirstObjectByType<TargetingRules>();
            if (!preview) preview = FindFirstObjectByType<PreviewHighlighter>();
            if (!board)   board   = FindFirstObjectByType<BoardGrid>();
        }

        private void OnEnable()
        {
            targetingRequest.OnRequest += BeginTargeting;

            if (inputBus != null)
            {
                inputBus.OnPointerMoved.AddListener(OnPointerMoved);
                inputBus.OnConfirm.AddListener(OnConfirm);
                inputBus.OnCancel.AddListener(OnCancel);
                inputBus.OnReset.AddListener(OnReset);
            }
        }
        private void OnDisable()
        {
            targetingRequest.OnRequest -= BeginTargeting;
            if (inputBus != null)
            {
                inputBus.OnPointerMoved.RemoveListener(OnPointerMoved);
                inputBus.OnConfirm.RemoveListener(OnConfirm);
                inputBus.OnCancel.RemoveListener(OnCancel);
                inputBus.OnReset.RemoveListener(OnReset);
            }
            preview?.Clear();
            ClearEligible();
        }

        private void BeginTargeting(AbilitySO ability, int? lane)
        {
            _ability = ability;
            _laneFilter = lane;
            _selected.Clear();
            BuildEligible();
            ShowPrompt(true);
        }

        private void BuildEligible()
        {
            ClearEligible();
            foreach (var c in FindObjectsByType<TargetCandidate>(FindObjectsSortMode.None))
            {
                if (_laneFilter.HasValue && c.lane != _laneFilter.Value) continue;

                bool ok = _ability.targetKind switch
                {
                    AbilitySO.TargetKind.UnitAlly  => c.isAlly,
                    AbilitySO.TargetKind.UnitEnemy => !c.isAlly,
                    AbilitySO.TargetKind.Tile      => true,
                    AbilitySO.TargetKind.Multi     => true,
                    _ => false
                };
                if (ok) { _eligible.Add(c); c.SetHighlight(true); }
            }
        }

        private void ClearEligible()
        {
            foreach (var e in _eligible) if (e) { e.SetHighlight(false); e.SetReticle(false); }
            _eligible.Clear();
        }

        // ------- Input Bus -------
        private void OnPointerMoved(Vector2 p)
        {
            _pointer = p;
            if (_ability == null) return;
            if (EventSystem.current && EventSystem.current.IsPointerOverGameObject()) { preview?.Clear(); return; }

            // Hover preview
            var (cand, tile) = RaycastCandidateAt(_pointer);
            if (!cand)
            {
                preview?.Clear();
                return;
            }

            var v = ValidateCandidate(cand, tile);
            preview?.ShowPreview(v.affected, v.valid);
        }

        private void OnConfirm()
        {
            if (_ability == null) return;
            if (EventSystem.current && EventSystem.current.IsPointerOverGameObject()) return;

            var (cand, tile) = RaycastCandidateAt(_pointer);
            if (!cand && _ability.targetKind != AbilitySO.TargetKind.Tile) return;

            var v = ValidateCandidate(cand, tile);
            if (!v.valid)
            {
                var msg = TargetingReasonText.ToText(v.reason);
                if (!string.IsNullOrEmpty(msg)) feedback?.Raise(msg);
                return;
            }

            // Multi-select?
            if (_ability.allowMultiple)
            {
                // add all affected units from this pick
                foreach (var go in v.affected)
                {
                    if (!_selected.Contains(go))
                        _selected.Add(go);
                }

                // reticle on last clicked
                if (cand) cand.SetReticle(true);

                // cap
                if (_selected.Count >= Mathf.Max(1, _ability.maxTargets))
                {
                    ConfirmNow();
                }
                else
                {
                    // keep previewing until player cancels or reaches cap
                    return;
                }
            }
            else
            {
                _selected.Clear();
                _selected.AddRange(v.affected.Count > 0 ? v.affected : new List<GameObject>());
                if (cand) cand.SetReticle(true);
                ConfirmNow();
            }
        }

        private void ConfirmNow()
        {
            ShowPrompt(false);
            preview?.Clear();
            ClearEligible();

            // For tile abilities you can compute tile from ray; here we keep unit-based
            targetingResult?.Raise(new List<GameObject>(_selected), null);
            _ability = null;
            _selected.Clear();
        }

        private void OnCancel()
        {
            if (_ability == null) return;
            ShowPrompt(false);
            preview?.Clear();
            ClearEligible();
            targetingResult?.Raise(null, null);
            _ability = null;
            _selected.Clear();
        }

        private void OnReset()
        {
            if (_ability == null) return;
            foreach (var c in _eligible) c.SetReticle(false);
            _selected.Clear();
            preview?.Clear();
        }

        // ------- Helpers -------
        private (TargetCandidate cand, TileRef? tile) RaycastCandidateAt(Vector2 screen)
        {
            var cam = Camera.main; if (!cam) return (null, null);

#if UNITY_2D
            var wp = cam.ScreenToWorldPoint(screen);
            var hit2D = Physics2D.OverlapPoint(wp);
            var cand2D = hit2D ? hit2D.GetComponentInParent<TargetCandidate>() : null;
            return (cand2D, null);
#else
            var ray = cam.ScreenPointToRay(screen);
            Physics.Raycast(ray, out var hit, 1000f);
            var cand = hit.collider ? hit.collider.GetComponentInParent<TargetCandidate>() : null;
            return (cand, null);
#endif
        }

        private TargetValidation ValidateCandidate(TargetCandidate cand, TileRef? tile)
        {
            if (_ability.targetKind == AbilitySO.TargetKind.Tile && tile.HasValue)
                return rules.ValidateTile(_ability, _laneFilter ?? 0, tile.Value);

            if (!cand) return default;
            return rules.ValidateUnit(_ability, _laneFilter ?? cand.lane, cand.gameObject);
        }

        private void ShowPrompt(bool on)
        {
            if (!prompt) return;
            prompt.alpha = on ? 1f : 0f;
            prompt.interactable = on;
            prompt.blocksRaycasts = on;
        }
    }
}
