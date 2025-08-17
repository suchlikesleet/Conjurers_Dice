using UnityEngine;
using UnityEngine.Events;

namespace ConjurerDice
{
    [CreateAssetMenu(menuName = "ConjurerDice/Events/TurnPhase")]
    public class TurnPhaseEventChannelSO : ScriptableObject
    {
        public UnityAction<TurnPhase> OnRaised;
        public void Raise(TurnPhase p) => OnRaised?.Invoke(p);
    }
}