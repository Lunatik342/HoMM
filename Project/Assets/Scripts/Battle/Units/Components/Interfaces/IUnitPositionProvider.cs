using RogueSharp;

namespace Battle.BattleArena.Pathfinding
{
    public interface IUnitPositionProvider
    {
        Cell OccupiedCell { get; }
    }
}