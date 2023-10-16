using System.Collections.Generic;
using System.Linq;
using Battle.BattleArena.Pathfinding;
using Battle.Units;
using Utilities;

namespace Battle.BattleFlow
{
    public class TurnsQueueService
    {
        private readonly IUnitsHolder _unitsHolder;

        private Queue<Unit> _queue = new ();

        public TurnsQueueService(IUnitsHolder unitsHolder)
        {
            _unitsHolder = unitsHolder;
        }

        public void InitializeFromStartingUnits()
        {
            var shuffledUnits = _unitsHolder.AllUnits.Shuffle().ToList();

            foreach (var unit in shuffledUnits)
            {
                _queue.Enqueue(unit);
            }
        }

        public Unit Dequeue()
        {
            var unit = _queue.Dequeue();
            _queue.Enqueue(unit);
            return unit;
        }
    }
}