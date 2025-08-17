// Scripts/Content/Events/TargetingFeedbackEventChannelSO.cs
using UnityEngine;
using UnityEngine.Events;

namespace ConjurerDice
{
    [CreateAssetMenu(menuName="ConjurerDice/Events/Targeting Feedback")]
    public class TargetingFeedbackEventChannelSO : ScriptableObject
    {
        public UnityAction<string> OnRaised;
        public void Raise(string msg) => OnRaised?.Invoke(msg);
    }
}