using UnityEngine;

namespace ConjurerDice
{
    [CreateAssetMenu(menuName="ConjurerDice/Dice/Dice Definition")]
    public class DiceDefinitionSO : ScriptableObject
    {
        [Header("Identity")]
        public string displayName = "Apprentice d6";
        public Sprite icon;
        public int sides = 6;             // 4,6,8,10,12,20 supported if you like

        [Header("Faces (length must match sides)")]
        public DiceFaceSO[] faces;

        [Header("Unlocks")]
        public int unlockChapter = 1;     // visible from this chapter onward
        public bool isDefaultUnlocked = true;

        [Header("Limits & Tags")]
        public int maxCopiesInLoadout = 1;
        public string[] tags;             // e.g., "Fire", "Control", "Economy"
    }
}