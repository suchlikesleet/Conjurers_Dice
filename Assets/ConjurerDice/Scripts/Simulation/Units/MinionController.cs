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
public void TakeDamage(int amount, object source)
{
    if (stats.HP <= 0) return; // Already dead
    
    var damage = Mathf.Max(0, amount - stats.DEF);
    stats.HP = Mathf.Max(0, stats.HP - damage);
    
    Debug.Log($"[Combat] Ally {name} takes {damage} damage (ATK:{amount} - DEF:{stats.DEF})! HP: {stats.HP}/{stats.MaxHP}");
    
    if (stats.HP <= 0)
    {
        OnDeath();
    }
}

private void OnDeath()
{
    Debug.Log($"[Combat] Ally {name} has fallen!");
    
    // Optional: Death effects
    // TODO: Add death animation/particles
    
    Destroy(gameObject, 0.1f);
}

public void ApplyStatus(StatusEffectSO effect)
{
    // TODO: Implement status effect system
    Debug.Log($"[Status] Ally {name} affected by {effect?.name}");
}

// Optional: Add attack capability for minions
public void AttackTarget(EnemyController target)
{
    if (target == null || !target.IsAlive) return;
    
    Debug.Log($"[Combat] {name} attacks {target.name} for {stats.ATK} damage!");
    target.TakeDamage(stats.ATK, this);
}
    }
}
