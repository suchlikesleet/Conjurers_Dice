using UnityEngine;
using UnityEngine.Events;

namespace ConjurerDice
{
    [CreateAssetMenu(menuName="ConjurerDice/Events/Open Dice Picker")]
    public class OpenDicePickerEventSO : ScriptableObject
    {
        public UnityAction<int /*chapter*/> OnRaised;
        public void Raise(int chapter) => OnRaised?.Invoke(chapter);
    }

    [CreateAssetMenu(menuName="ConjurerDice/Events/Dice Loadout Confirmed")]
    public class DiceLoadoutConfirmedEventSO : ScriptableObject
    {
        public UnityAction OnRaised;
        public void Raise() => OnRaised?.Invoke();
    }
}