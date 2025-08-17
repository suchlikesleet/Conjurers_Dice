using UnityEngine;
using System.Linq;

namespace ConjurerDice
{
    [CreateAssetMenu(menuName="ConjurerDice/Dice/Dice Library")]
    public class DiceLibrarySO : ScriptableObject
    {
        public DiceDefinitionSO[] allDice;

        public DiceDefinitionSO[] GetUnlockedForChapter(int chapter)
        {
            return allDice.Where(d =>
                d != null &&
                (d.isDefaultUnlocked || d.unlockChapter <= chapter)
            ).ToArray();
        }
    }
}