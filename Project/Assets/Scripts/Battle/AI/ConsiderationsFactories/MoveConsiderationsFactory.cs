using System.Collections.Generic;
using System.Linq;
using Algorithms.RogueSharp;
using Battle.AI.Considerations;
using Battle.Arena.Map;
using Battle.Units;
using Battle.Units.Creation;

namespace Battle.AI.ConsiderationsFactories
{
    public class MoveConsiderationsFactory
    {
        private readonly IUnitsHolder _unitsHolder;
        private readonly IMapHolder _mapHolder;

        public MoveConsiderationsFactory(IUnitsHolder unitsHolder, IMapHolder mapHolder)
        {
            _unitsHolder = unitsHolder;
            _mapHolder = mapHolder;
        }
        
        public Dictionary<Cell, IConsideration> AddNewConsiderationsToList(Unit unit, List<Cell> reachableCells, List<IConsideration> considerationsList)
        {
            var considerationsForPosition = new Dictionary<Cell, IConsideration>();
            
            var mapSize = _mapHolder.Map.Height * _mapHolder.Map.Width;
            
            var allEnemyUnits = _unitsHolder.GetAllAliveUnitsOfTeam(unit.Team.GetOppositeTeam());
            var enemyReachableCells = allEnemyUnits.Select(u => u.MovementController.GetReachableCells()).ToArray();

            foreach (var targetCell in reachableCells)
            {
                var consideration = new MoveConsideration(unit, targetCell, reachableCells, allEnemyUnits, enemyReachableCells, mapSize);
                
                considerationsForPosition.Add(targetCell, consideration);
                considerationsList.Add(consideration);
            }

            return considerationsForPosition;
        }
    }
}