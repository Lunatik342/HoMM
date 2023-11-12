using System.Collections.Generic;
using Algorithms.RogueSharp;
using Battle.Arena.Misc;
using Utilities;

namespace Algorithms
{
    public class ReachableForAttackCellsFinder
    {
        private const float _tolerance = 0.01f;
        
        public static bool CanReachCellForMeleeAttack(Cell target, List<Cell> reachableCells, out List<Cell> adjacentToEnemyReachableCells)
        {
            adjacentToEnemyReachableCells = new List<Cell>();
            
            foreach (var reachableCell in reachableCells)
            {
                if (CellsUtilities.CalculateDistance(reachableCell, target) < BattleArenaConstants.DiagonalMovementCost + _tolerance)
                {
                    adjacentToEnemyReachableCells.Add(reachableCell);
                }
            }

            return adjacentToEnemyReachableCells.Count > 0;
        }
        
        public static bool CanReachCellForMeleeAttack(Cell target, List<Cell> reachableCells)
        {
            foreach (var reachableCell in reachableCells)
            {
                if (CellsUtilities.CalculateDistance(reachableCell, target) < BattleArenaConstants.DiagonalMovementCost + _tolerance)
                {
                    return true;
                }
            }

            return false;
        }
    }
}