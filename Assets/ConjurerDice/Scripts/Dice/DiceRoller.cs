using UnityEngine;

namespace ConjurerDice
{
    public class DiceRoller : MonoBehaviour
    {
        public DiceFaceSO Roll(DiceSO dice)
        {
            if (dice == null || dice.faces == null || dice.faces.Length == 0) return null;
            int i = Random.Range(0, dice.faces.Length);
            return dice.faces[i];
        }
    }
}