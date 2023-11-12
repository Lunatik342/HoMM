using System;
using Algorithms.RogueSharp;
using Battle.Arena.Misc;

namespace Utilities
{
    public static class CellsUtilities
    {
        public static float CalculateDistance(ICell source, ICell destination)
        {
            int dx = Math.Abs(source.X - destination.X);
            int dy = Math.Abs(source.Y - destination.Y);

            int dMin = Math.Min(dx, dy);
            int dMax = Math.Max(dx, dy);

            return (dMin * BattleArenaConstants.DiagonalMovementCost) + (dMax - dMin);
        }
    }
}