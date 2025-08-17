namespace ConjurerDice {
    public interface IUnit {

bool IsAlly { get; }
UnitStats Stats { get; }
TileRef Tile { get; }
void Teleport(TileRef t);
void TakeDamage(int amount, object source);
void ApplyStatus(StatusEffectSO effect);

    }
}

namespace ConjurerDice
{
    public interface IEnemyActor
    {
        bool IsAlive { get; }
        void TickEnemyTurn(IBoardGrid board);
    }
}
