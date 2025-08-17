using UnityEngine;

namespace ConjurerDice {
    public class MovementSystem : MonoBehaviour {

public static int CalculateMovement(int diceValue, UnitStats stats) {
    switch (stats.MOVType) {
        case MovementType.Fixed: return Mathf.Max(0, stats.FixedMov);
        case MovementType.Standard: return diceValue;
        case MovementType.Slow: return Mathf.FloorToInt(diceValue * 0.5f);
        case MovementType.Fast: return Mathf.CeilToInt(diceValue * 1.5f);
        case MovementType.Capped: return Mathf.Min(diceValue, stats.MovCap);
        default: return diceValue;
    }
}
public static int MoveByPoints(MonoBehaviour unitBehaviour, int lane, int points) { /* TODO */ return points; }

    }
}
