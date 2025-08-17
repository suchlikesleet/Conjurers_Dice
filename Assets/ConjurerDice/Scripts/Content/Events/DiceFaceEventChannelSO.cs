using UnityEngine;
using UnityEngine.Events;

namespace ConjurerDice {
    [CreateAssetMenu(menuName="ConjurerDice/Events/DiceFace")]
    public class DiceFaceEventChannelSO : ScriptableObject {
        public UnityAction<DiceFaceSO> OnRaised;
        public void Raise(DiceFaceSO face) => OnRaised?.Invoke(face);
    }
}
