using System.Collections.Generic;
using Algorithms.RogueSharp;
using Battle.Units;
using Utilities;

namespace Algorithms
{
    public class FlyingUnitReachableCellsFinder<TCell> where TCell : class, ICell
    {
        public static List<Cell> GetReachableCells(TCell source, IMap<TCell> map, Unit pathingAgent, int maxDistanceFormStart)
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

                    if (CellsUtilities.CalculateDistance(source, cell) < maxDistanceFormStart)
                    {
                        result.Add(cell.GetLogicalCell());
                    }
                }
            }

            return result;
        }
    }
}