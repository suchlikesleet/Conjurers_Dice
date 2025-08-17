using UnityEngine;

namespace ConjurerDice {
    [CreateAssetMenu(menuName="ConjurerDice/EnemySO")]
    public class EnemySO : ScriptableObject {

public string displayName;
public GameObject prefab;
public UnitStats baseStats;
public TagSetSO tagSet;
public UnityEngine.Object behaviorTree;

    }
}
