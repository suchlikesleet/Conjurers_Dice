using UnityEngine;

namespace ConjurerDice {
    [CreateAssetMenu(menuName="ConjurerDice/MinionSO")]
    public class MinionSO : ScriptableObject {

public string displayName;
public GameObject prefab;
public UnitStats baseStats;
public TagSetSO tagSet;
public bool spawnsAtFront = false;

    }
}
