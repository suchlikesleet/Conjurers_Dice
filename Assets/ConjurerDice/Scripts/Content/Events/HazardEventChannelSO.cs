using UnityEngine;
using UnityEngine.Events;

namespace ConjurerDice {
    [System.Serializable] public struct HazardInfo { public TileRef tile; public HazardDefinitionSO def; }
    [CreateAssetMenu(menuName="ConjurerDice/Events/Hazard")]
    public class HazardEventChannelSO : ScriptableObject {
        public UnityAction<HazardInfo> OnRaised;
        public void Raise(HazardInfo info) => OnRaised?.Invoke(info);
    }
}
