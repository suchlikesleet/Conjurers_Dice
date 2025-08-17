using UnityEngine;

namespace ConjurerDice {
    public class Bootstrap : MonoBehaviour {

[SerializeField] private GameStateMachine gsm;
private void Awake() { if (gsm == null) gsm = FindAnyObjectByType<GameStateMachine>(); }
/*private void Start() { gsm?.Set(GameState.Title); }*/

    }
}
