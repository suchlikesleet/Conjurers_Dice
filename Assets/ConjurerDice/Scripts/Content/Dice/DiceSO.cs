using UnityEngine;

namespace ConjurerDice {
    [CreateAssetMenu(menuName="ConjurerDice/DiceSO")]
    public class DiceSO : ScriptableObject {

public DiceFaceSO[] faces = new DiceFaceSO[6];
public string diceName = "Default";

    }
}
