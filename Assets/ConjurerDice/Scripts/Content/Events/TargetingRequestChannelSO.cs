using UnityEngine;
using UnityEngine.Events;

namespace ConjurerDice
{
    [CreateAssetMenu(menuName="ConjurerDice/Events/Targeting Request")]
    public class TargetingRequestChannelSO : ScriptableObject
    {
        // lane is optional; when set we filter to that lane
        public UnityAction<AbilitySO, int?> OnRequest;
        public void Raise(AbilitySO ability, int? lane) => OnRequest?.Invoke(ability, lane);
    }
}
