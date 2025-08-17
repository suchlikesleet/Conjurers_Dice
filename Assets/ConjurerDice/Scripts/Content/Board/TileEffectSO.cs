using UnityEngine;

namespace ConjurerDice {
    [CreateAssetMenu(menuName="ConjurerDice/TileEffectSO")]
    public class TileEffectSO : ScriptableObject {

public string effectName;
public int magnitude;
public int durationTurns = 1;

    }
}
