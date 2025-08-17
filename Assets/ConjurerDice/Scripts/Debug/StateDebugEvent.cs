using Sirenix.OdinInspector;
using UnityEngine;

namespace ConjurerDice
{
    public class StateDebugEvent : MonoBehaviour
    {
        [SerializeField]private GameStateEventChannelSO gameStateRequest;
        [Button]
        public void CallStateDebug()
        {
            gameStateRequest.Raise(GameState.Title);
        }
    }
}
