using UnityEngine;
using UnityEngine.Events;

namespace ConjurerDice {
    [System.Serializable] public struct UnitRef { public GameObject go; }
    [CreateAssetMenu(menuName="ConjurerDice/Events/UnitRef")]
    public class UnitRefEventChannelSO : ScriptableObject {
        public UnityAction<UnitRef> OnRaised;
        public void Raise(UnitRef value) => OnRaised?.Invoke(value);
    }
}
