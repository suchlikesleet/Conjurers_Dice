using UnityEngine;

namespace ConjurerDice {
    [CreateAssetMenu(menuName="ConjurerDice/EncounterSO")]
    public class EncounterSO : ScriptableObject {

public string encounterName;
public int lanes = 3;
public int laneLength = 8;
public EnemySO[] enemies;
public HazardDefinitionSO[] hazards;

    }
}
