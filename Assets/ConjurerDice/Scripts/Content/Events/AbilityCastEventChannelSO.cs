using UnityEngine;
using UnityEngine.Events;

namespace ConjurerDice {
    [CreateAssetMenu(menuName="ConjurerDice/Events/AbilityCast")]
    public class AbilityCastEventChannelSO : ScriptableObject {
        public UnityAction<AbilityCastContext> OnRaised;
        public void Raise(AbilityCastContext ctx) => OnRaised?.Invoke(ctx);
    }
}
