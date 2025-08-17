using UnityEngine;

namespace ConjurerDice {
    [CreateAssetMenu(menuName="ConjurerDice/DiceFaceSO")]
    public class DiceFaceSO : ScriptableObject {

public enum DiceFaceType { Number, Minion, Ability, Hybrid }
public DiceFaceType type;
[Range(0,6)] public int numberValue = 0;
public MinionSO minion;
public AbilitySO ability;
public DiceFaceSO halfA;
public DiceFaceSO halfB;
[Min(0)] public int manaCost = 1;
public Sprite icon;

    }
}
