using System.Collections.Generic;
using Battle.AI.Considerations;
using Battle.Units;
using Utilities;

namespace Battle.AI.ConsiderationsFactories
{
    public class ConsiderationsFactory
    {
        private readonly AttackConsiderationsFactory _attackConsiderationsFactory;
        private readonly MoveConsiderationsFactory _moveConsiderationsFactory;

        public ConsiderationsFactory(AttackConsiderationsFactory attackConsiderationsFactory, 
            MoveConsiderationsFactory moveConsiderationsFactory)
        {
            _attackConsiderationsFactory = attackConsiderationsFactory;
            _moveConsiderationsFactory = moveConsiderationsFactory;
        }

        public List<IConsideration> Create(Unit unit)
        {
            var considerations = new List<IConsideration>();
            var reachableCells = unit.MovementController.GetReachableCells();
            
            var moveConsiderations = _moveConsiderationsFactory.AddNewConsiderationsToList(unit, reachableCells, considerations);
            moveConsiderations.Foreach( c => c.Value.Consider());
            
            _attackConsiderationsFactory.AddConsiderationsToList(unit, reachableCells, considerations, moveConsiderations);

            return considerations;
        }
    }
}