using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace ConjurerDice
{
    [CreateAssetMenu(menuName="ConjurerDice/Events/Targeting Result")]
    public class TargetingResultEventChannelSO : ScriptableObject
    {
        public UnityAction<List<GameObject>, TileRef?> OnResult; // units and/or tile
        public void Raise(List<GameObject> units, TileRef? tile) => OnResult?.Invoke(units, tile);
    }
}