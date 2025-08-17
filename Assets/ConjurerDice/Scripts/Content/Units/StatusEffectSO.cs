using UnityEngine;

namespace ConjurerDice {
    [CreateAssetMenu(menuName="ConjurerDice/StatusEffectSO")]
    public class StatusEffectSO : ScriptableObject {

public enum StatusType { Shield, Haste, Regen, Fortify, Overcharge, Burn, Freeze, Poison, Slow, Root }
public StatusType type;
public int magnitude = 1;
public int durationTurns = 1;

    }
}
