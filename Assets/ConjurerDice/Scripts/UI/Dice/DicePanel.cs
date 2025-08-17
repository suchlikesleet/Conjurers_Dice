using UnityEngine;

namespace ConjurerDice {
    public class DicePanel : MonoBehaviour {

[SerializeField] private DiceFaceEventChannelSO onDieRolled;
private void OnEnable() { if (onDieRolled != null) onDieRolled.OnRaised += ShowFace; }
private void OnDisable() { if (onDieRolled != null) onDieRolled.OnRaised -= ShowFace; }
private void ShowFace(DiceFaceSO face) { /* TODO: display face */ }

    }
}
