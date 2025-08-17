using UnityEngine;
using UnityEngine.Events;

namespace ConjurerDice {
    [CreateAssetMenu(menuName="ConjurerDice/Events/Lane")]
    public class LaneEventChannelSO : ScriptableObject {
        public UnityAction<int> OnRaised;
        public void Raise(int laneIndex) => OnRaised?.Invoke(laneIndex);
    }
}
