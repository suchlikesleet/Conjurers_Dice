using UnityEngine;
using UnityEngine.Events;

namespace ConjurerDice
{
    [CreateAssetMenu(menuName = "ConjurerDice/Events/GameState Event Channel")]
    public class GameStateEventChannelSO : ScriptableObject
    {
        public UnityAction<GameState> OnEventRaised;

        public void Raise(GameState state)
        {
            OnEventRaised?.Invoke(state);
        }
    }
}