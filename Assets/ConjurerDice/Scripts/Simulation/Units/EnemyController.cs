using System.Linq;
using UnityEngine;

namespace ConjurerDice {
    public class EnemyController : MonoBehaviour {

public bool isAlly = false;
public UnitStats stats;
public TileRef tile;
//public bool IsAlive => stats.HP > 0;
public bool IsAlly => isAlly;
public UnitStats Stats => stats;
public TileRef Tile => tile;
public void Teleport(TileRef t) { tile = t; }

// Add these implementations to EnemyController.cs

public bool IsAlive => stats.HP > 0;

public void TickEnemyTurn(IBoardGrid board)
{
    if (!IsAlive) return;
    
    Debug.Log($"[EnemyAI] {name} taking turn at L{tile.lane}:{tile.index}");
    
    // Find all allies in the same lane
    var alliesInLane = FindObjectsByType<MinionController>(FindObjectsSortMode.None)
        .Where(ally => ally != null && ally.tile.lane == tile.lane && ally.stats.HP > 0)
        .OrderBy(ally => ally.tile.index) // Sort by position (back to front)
        .ToList();
    
    if (alliesInLane.Count == 0)
    {
        Debug.Log($"[EnemyAI] {name} - No allies in lane {tile.lane}, moving toward player base");
        MoveTowardPlayerBase();
        return;
    }
    
    // Find the closest ally (lowest index = closest to player base)
    var closestAlly = alliesInLane.First();
    int distanceToAlly = Mathf.Abs(tile.index - closestAlly.tile.index);
    
    Debug.Log($"[EnemyAI] {name} - Closest ally: {closestAlly.name} at distance {distanceToAlly}, attack range: {stats.RNG}");
    
    // Can we attack?
    if (distanceToAlly <= stats.RNG)
    {
        AttackTarget(closestAlly);
    }
    else
    {
        // Move toward the ally
        MoveTowardTarget(closestAlly.tile.index);
    }
}

private void AttackTarget(MinionController target)
{
    if (target == null || target.stats.HP <= 0) return;
    
    Debug.Log($"[Combat] {name} attacks {target.name} for {stats.ATK} damage!");
    target.TakeDamage(stats.ATK, this);
    
    // Optional: Add attack animation/effects here
}

private void MoveTowardTarget(int targetIndex)
{
    // Move one step toward the target
    int currentIndex = tile.index;
    int newIndex;
    
    if (targetIndex < currentIndex)
    {
        // Target is behind us (toward player base) - move backward
        newIndex = currentIndex - 1;
    }
    else if (targetIndex > currentIndex)
    {
        // Target is ahead of us - move forward
        newIndex = currentIndex + 1;
    }
    else
    {
        // Already at target position (shouldn't happen)
        return;
    }
    
    // Clamp to board bounds
    var board = FindFirstObjectByType<BoardGrid>();
    if (board != null)
    {
        newIndex = Mathf.Clamp(newIndex, 0, board.LaneLength - 1);
    }
    
    Debug.Log($"[EnemyAI] {name} moves from {tile.lane}:{currentIndex} to {tile.lane}:{newIndex}");
    Teleport(new TileRef(tile.lane, newIndex));
}

private void MoveTowardPlayerBase()
{
    // Move toward index 0 (player base)
    int newIndex = Mathf.Max(0, tile.index - 1);
    
    if (newIndex != tile.index)
    {
        Debug.Log($"[EnemyAI] {name} advances toward player base: {tile.lane}:{tile.index} → {tile.lane}:{newIndex}");
        Teleport(new TileRef(tile.lane, newIndex));
    }
    else
    {
        Debug.Log($"[EnemyAI] {name} has reached player base! GAME OVER!");
        // TODO: Trigger game over condition
    }
}

public void TakeDamage(int amount, object source)
{
    if (!IsAlive) return;
    
    var damage = Mathf.Max(0, amount - stats.DEF);
    stats.HP = Mathf.Max(0, stats.HP - damage);
    
    Debug.Log($"[Combat] {name} takes {damage} damage (ATK:{amount} - DEF:{stats.DEF})! HP: {stats.HP}/{stats.MaxHP}");
    
    if (stats.HP <= 0)
    {
        OnDeath();
    }
}

private void OnDeath()
{
    Debug.Log($"[Combat] {name} has been defeated!");
    
    // Optional: Death effects, loot, etc.
    // TODO: Add death animation/particles
    
    // Remove from the game
    Destroy(gameObject, 0.1f); // Small delay for effects
}

// Add missing ApplyStatus method
public void ApplyStatus(StatusEffectSO effect)
{
    // TODO: Implement status effect system
    Debug.Log($"[Status] {name} affected by {effect?.name}");
}

public bool LaneIsClogged() => false; // TODO
public Transform FindBestTargetInLane() => null;


    }
}
