using System.Collections.Generic;
using System.Linq;
using Battle.BattleArena.Pathfinding;
using Battle.Units;
using Battle.Units.StatsSystem;
using Utilities;

namespace Battle.BattleFlow
{
    public class TurnsQueueService
    {
        private readonly IUnitsHolder _unitsHolder;

        private List<Unit> _queue = new ();

        public TurnsQueueService(IUnitsHolder unitsHolder)
        {
            _unitsHolder = unitsHolder;
        }

        public void InitializeFromStartingUnits()
        {
            var shuffledUnits = _unitsHolder.GetAllUnits().OrderByDescending(u => u.StatsProvider.GetStatValue(StatType.Initiative)).ToList();

            foreach (var unit in shuffledUnits)
            {
                _queue.Add(unit);
                unit.Health.UnitDied += () => { _queue.Remove(unit);};
            }
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