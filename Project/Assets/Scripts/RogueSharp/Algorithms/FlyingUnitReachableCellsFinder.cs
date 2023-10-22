using System;
using System.Collections.Generic;
using Battle.BattleArena.Pathfinding;

namespace RogueSharp.Algorithms
{
    public class FlyingUnitReachableCellsFinder<TCell> where TCell : ICell
    {
        private readonly double _diagonalCost;

        public FlyingUnitReachableCellsFinder( double diagonalCost )
        {
            _diagonalCost = diagonalCost;
        }

        public List<Cell> GetReachableCells(TCell source, IMap<TCell> map, Unit pathingAgent, int maxDistanceFormStart)
        {
            List<Cell> result = new List<Cell>();
            
            for (int i = 0; i < map.Width; i++)
            {
                for (int j = 0; j < map.Height; j++)
                {
                    var cell = map[i, j];

                    if (!cell.CanPlaceUnit(pathingAgent))
                    {
                        continue;
                    }

                    if (CalculateDistance(source, cell) < maxDistanceFormStart)
                    {
                        result.Add(cell.GetLogicalCell());
                    }
                }
            }

            return result;
        }

        private double CalculateDistance(TCell source, TCell destination)
        {
            int dx = Math.Abs(source.X - destination.X);
            int dy = Math.Abs(source.Y - destination.Y);

            int dMin = Math.Min(dx, dy);
            int dMax = Math.Max(dx, dy);

            return (dMin * _diagonalCost) + (dMax - dMin);
        }
    }
}