using UnityEngine;
using UnityEngine.Events;

namespace ConjurerDice {
    [CreateAssetMenu(menuName="ConjurerDice/Events/Void")]
    public class VoidEventChannelSO : ScriptableObject {
        public UnityAction OnRaised;
        public void Raise() => OnRaised?.Invoke();
    }
}
