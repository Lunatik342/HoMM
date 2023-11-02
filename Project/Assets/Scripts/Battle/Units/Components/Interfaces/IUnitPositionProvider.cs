using Algorithms.RogueSharp;

namespace Battle.Units.Components.Interfaces
{
    public interface IUnitPositionProvider
    {
        Cell OccupiedCell { get; }
    }
}