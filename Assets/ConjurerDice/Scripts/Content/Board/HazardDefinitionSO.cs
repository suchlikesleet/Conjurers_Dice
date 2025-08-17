using UnityEngine;

namespace ConjurerDice {
    [CreateAssetMenu(menuName="ConjurerDice/HazardDefinitionSO")]
    public class HazardDefinitionSO : ScriptableObject {

public enum Type { Fire, Ice, Poison, Spikes }
public Type type;
public int damage = 1;
public int durationTurns = 2;
public bool ticksOnEnter = true;
public bool ticksOnEnd = true;

    }
}
