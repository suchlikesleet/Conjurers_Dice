// Scripts/Simulation/Abilities/TargetingReasonText.cs
namespace ConjurerDice
{
    public static class TargetingReasonText
    {
        public static string ToText(TargetInvalidReason r) => r switch
        {
            TargetInvalidReason.WrongAllegiance => "Wrong target type.",
            TargetInvalidReason.WrongLane       => "Target not in lane.",
            TargetInvalidReason.OutOfRange      => "Out of range.",
            TargetInvalidReason.Blocked         => "Path blocked.",
            TargetInvalidReason.MaxTargetsReached => "Max targets reached.",
            _ => ""
        };
    }
}