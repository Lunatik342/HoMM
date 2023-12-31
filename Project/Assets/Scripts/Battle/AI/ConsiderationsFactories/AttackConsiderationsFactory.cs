using System.Collections.Generic;
using Algorithms;
using Algorithms.RogueSharp;
using Battle.AI.Considerations;
using Battle.Units;
using Battle.Units.Creation;

namespace Battle.AI.ConsiderationsFactories
{
    public class AttackConsiderationsFactory
    {
        private readonly IUnitsHolder _unitsHolder;

        public AttackConsiderationsFactory(IUnitsHolder unitsHolder)
        {
            _unitsHolder = unitsHolder;
        }

        public void AddConsiderationsToList(Unit unit, 
            List<Cell> reachableCells, 
            List<IConsideration> considerationsList, 
            Dictionary<Cell, IConsideration> considerationsForMovement)
        {
            var allEnemyUnits = _unitsHolder.GetAllAliveUnitsOfTeam(unit.Team.GetOppositeTeam());

            foreach (var enemyUnit in allEnemyUnits)
            {
                if (ReachableForAttackCellsFinder.CanReachCellForMeleeAttack(enemyUnit.PositionProvider.OccupiedCell, 
                        unit.PositionProvider.OccupiedCell, reachableCells, out var canAttackFromCells))
                {
                    foreach (var attackFromCell in canAttackFromCells)
                    {
                        considerationsList.Add(new AttackConsideration(unit, enemyUnit, attackFromCell, considerationsForMovement[attackFromCell]));
                    }
                }
            }
        }
    }
}
