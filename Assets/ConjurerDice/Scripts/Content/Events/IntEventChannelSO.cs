using UnityEngine;
using UnityEngine.Events;

namespace ConjurerDice {
    [CreateAssetMenu(menuName="ConjurerDice/Events/Int")]
    public class IntEventChannelSO : ScriptableObject {
        public UnityAction<int> OnRaised;
        public void Raise(int value) => OnRaised?.Invoke(value);
    }
}
