using UnityEngine;

namespace ConjurerDice {
    public class ForkEvaluator : MonoBehaviour {

public bool IsVisible(ForkDataSO fork) {
    if (fork == null) return false;
    if (fork.conditions == null || fork.conditions.Length == 0) return true;
    foreach (var c in fork.conditions) if (c != null && !c.IsMet()) return false;
    return true;
}

    }
}
