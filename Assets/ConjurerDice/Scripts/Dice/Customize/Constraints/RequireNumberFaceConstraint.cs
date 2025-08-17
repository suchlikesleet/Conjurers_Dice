// Assets/ConjurerDice/Scripts/Dice/Customize/Constraints/RequireNumberFaceConstraint.cs
using UnityEngine;
using System.Linq;

namespace ConjurerDice
{
    [CreateAssetMenu(menuName="ConjurerDice/Dice Constraints/Require Number Face (Min 1)")]
    public class RequireNumberFaceConstraint : DiceConstraintSO
    {
        //[SerializeField] private string numberTypeName = "Number"; // switch to enum if you have one

        public override string Validate(PlayerDiceLoadoutSO loadout, DiceDefinitionSO candidateToAdd) => null;

        public override string ValidateFinal(PlayerDiceLoadoutSO loadout)
        {
            if (loadout == null || loadout.chosenDice == null || loadout.chosenDice.Count == 0)
                return "Pick at least one die.";

            bool hasNumber = loadout.chosenDice.Any(d =>
                d && d.faces != null && d.faces.Any(f => f && IsNumberFace(f)));

            return hasNumber ? null : "Include at least one die that has a Number face.";
        }

        private bool IsNumberFace(DiceFaceSO face)
        {
            // If you have an enum: return face.type == DiceFaceType.Number;
            return face.type == DiceFaceSO.DiceFaceType.Number;
            //return face.faceKindName == numberTypeName || face.name.Contains(numberTypeName);
            
        }
    }
}