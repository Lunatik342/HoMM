using System.Collections.Generic;
using System.Linq;
using Battle.StatsSystem;
using Battle.Units;
using Battle.Units.Creation;
using UnityEngine;

namespace Battle.BattleFlow
{
    public class UnitsQueueService
    {
        private readonly IUnitsHolder _unitsHolder;

        private List<Unit> _initialQueue = new ();
        private List<Unit> _currentTurnQueue = new ();

        public UnitsQueueService(IUnitsHolder unitsHolder)
        {
            _unitsHolder = unitsHolder;
        }

        public void InitializeFromStartingUnits()
        {
            var orderedUnits = _unitsHolder.GetAllUnits().OrderByDescending(u =>
            {
                var randomSpread = Random.Range(-0.1f, 0.1f);
                return u.StatsProvider.GetStatValue(StatType.Initiative) + randomSpread;
            }).ToList();

            foreach (var unit in orderedUnits)
            {
                _initialQueue.Add(unit);
                unit.Health.UnitDied += RemoveFromQueue;
            }
        }

        public Unit Dequeue()
        {
            var unit = _initialQueue[0];
            _initialQueue.RemoveAt(0);
            _initialQueue.Add(unit);
            return unit;
        }

        private void RemoveFromQueue(Unit diedUnit)
        {
            _initialQueue.Remove(diedUnit);
            diedUnit.Health.UnitDied -= RemoveFromQueue;
        }
    }
}