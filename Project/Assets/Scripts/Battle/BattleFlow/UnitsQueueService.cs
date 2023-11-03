using System.Collections.Generic;
using System.Linq;
using Battle.StatsSystem;
using Battle.Units;
using Battle.Units.Creation;
using Utilities;

namespace Battle.BattleFlow
{
    public class UnitsQueueService
    {
        private readonly IUnitsHolder _unitsHolder;

        private List<Unit> _queue = new ();

        public UnitsQueueService(IUnitsHolder unitsHolder)
        {
            _unitsHolder = unitsHolder;
        }

        public void InitializeFromStartingUnits()
        {
            var shuffledUnits = _unitsHolder.GetAllUnits().OrderByDescending(u => u.StatsProvider.GetStatValue(StatType.Initiative)).ToList();

            foreach (var unit in shuffledUnits)
            {
                _queue.Add(unit);
                unit.Health.UnitDied += RemoveFromQueue;
            }
        }

        private void RemoveFromQueue(Unit diedUnit)
        {
            _queue.Remove(diedUnit);
            diedUnit.Health.UnitDied -= RemoveFromQueue;
        }

        public Unit Dequeue()
        {
            var unit = _queue[0];
            _queue.RemoveAt(0);
            _queue.Add(unit);
            return unit;
        }
    }
}