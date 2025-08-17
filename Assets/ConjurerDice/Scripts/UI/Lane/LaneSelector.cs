using UnityEngine;

namespace ConjurerDice {
    public class LaneSelector : MonoBehaviour {

[SerializeField] private LaneEventChannelSO onLaneSelected;
public void SelectLane(int index) { onLaneSelected?.Raise(index); }

    }
}
