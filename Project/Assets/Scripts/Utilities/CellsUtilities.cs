using System;
using Battle.BattleArena.Pathfinding;
using RogueSharp;

namespace Utilities
{
    public static class CellsUtilities
    {
        public static double CalculateDistance(ICell source, ICell destination)
        {
            int dx = Math.Abs(source.X - destination.X);
            int dy = Math.Abs(source.Y - destination.Y);

            int dMin = Math.Min(dx, dy);
            int dMax = Math.Max(dx, dy);

            return (dMin * PathfindingService.DiagonalMovementCost) + (dMax - dMin);
        }
    }
}