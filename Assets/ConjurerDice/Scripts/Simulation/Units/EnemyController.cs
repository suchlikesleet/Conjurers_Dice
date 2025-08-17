using UnityEngine;

namespace ConjurerDice {
    public class EnemyController : MonoBehaviour {

public bool isAlly = false;
public UnitStats stats;
public TileRef tile;
public bool IsAlly => isAlly;
public UnitStats Stats => stats;
public TileRef Tile => tile;
public void Teleport(TileRef t) { tile = t; }
public void TakeDamage(int amount, object source) { /* TODO */ }
public void ApplyStatus(StatusEffectSO effect) { /* TODO */ }
public bool LaneIsClogged() => false; // TODO
public Transform FindBestTargetInLane() => null; // TODO

    }
}
