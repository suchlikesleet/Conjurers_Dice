using UnityEngine;
using UnityEngine.Events;

namespace ConjurerDice {
    public class GameStateMachine : MonoBehaviour {
        //[SerializeField] public GameState current = GameState.Boot;
        //[SerializeField] public GameStateEventChannelSO onStateChanged;
        [Header("Event Channels")]
        [SerializeField] private GameStateEventChannelSO gameStateChanged;

        public GameState CurrentState { get; private set; } = GameState.Boot;

        private void Start()
        {
            // First state on boot
            ChangeState(GameState.MainMenu);
        }

        public void ChangeState(GameState newState)
        {
            if (newState == CurrentState)
                return;

            CurrentState = newState;
            gameStateChanged.Raise(newState);

#if UNITY_EDITOR
            //Debug.Log($"[GameStateMachine] Changed to {newState}");
#endif
        }
        
        /*public void Set(GameState next) {
            if (current == next) return;
            current = next;
            onStateChanged?.Raise(current);
        }*/
        //public GameState Current => current;
    }
}
