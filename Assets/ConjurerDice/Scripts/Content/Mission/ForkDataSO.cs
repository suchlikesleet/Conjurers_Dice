using UnityEngine;

namespace ConjurerDice {
    [CreateAssetMenu(menuName="ConjurerDice/ForkDataSO")]
    public class ForkDataSO : ScriptableObject {

public enum ForkType { StaticMap, NPCTriggered, Hidden, EventBased }
public ForkType type;
public string description;
public ForkConditionSO[] conditions;
public RewardTableSO rewards;

    }
}
