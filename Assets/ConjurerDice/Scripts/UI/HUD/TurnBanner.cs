using UnityEngine;

namespace ConjurerDice {
    public class TurnBanner : MonoBehaviour {

[SerializeField] private IntEventChannelSO onTurnStarted;
[SerializeField] private UnityEngine.UI.Text text;
private void OnEnable() { if (onTurnStarted != null) onTurnStarted.OnRaised += UpdateBanner; }
private void OnDisable() { if (onTurnStarted != null) onTurnStarted.OnRaised -= UpdateBanner; }
private void UpdateBanner(int turn) { if (text) text.text = $"Turn {turn}"; }

    }
}
