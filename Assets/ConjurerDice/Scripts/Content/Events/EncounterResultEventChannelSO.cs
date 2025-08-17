using UnityEngine;
using UnityEngine.Events;

namespace ConjurerDice {
    [System.Serializable] public struct EncounterResult { public bool Win; }
    [CreateAssetMenu(menuName="ConjurerDice/Events/EncounterResult")]
    public class EncounterResultEventChannelSO : ScriptableObject {
        public UnityAction<EncounterResult> OnRaised;
        public void Raise(EncounterResult r) => OnRaised?.Invoke(r);
    }
}
