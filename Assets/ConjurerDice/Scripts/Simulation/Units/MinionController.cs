using UnityEngine;

namespace ConjurerDice {
    public class MinionController : MonoBehaviour {

public bool isAlly = true;
public UnitStats stats;
public TileRef tile;
public bool SpawnsAtFront;
public bool IsAlly => isAlly;
public UnitStats Stats => stats;
public TileRef Tile => tile;
public void Teleport(TileRef t) { tile = t; }
public void TakeDamage(int amount, object source) { /* TODO */ }
public void ApplyStatus(StatusEffectSO effect) { /* TODO */ }

    }
}
