using UnityEngine;

namespace ConjurerDice {
    [CreateAssetMenu(menuName="ConjurerDice/BoardConfigSO")]
    public class BoardConfigSO : ScriptableObject {

[Min(1)] public int lanes = 3;
[Min(1)] public int laneLength = 8;
public HazardDefinitionSO[] defaultHazards;

    }
}
