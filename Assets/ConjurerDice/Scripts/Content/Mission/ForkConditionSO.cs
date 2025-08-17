using UnityEngine;

namespace ConjurerDice {
    [CreateAssetMenu(menuName="ConjurerDice/ForkConditionSO")]
    public class ForkConditionSO : ScriptableObject {

public virtual bool IsMet() => false;

    }
}
