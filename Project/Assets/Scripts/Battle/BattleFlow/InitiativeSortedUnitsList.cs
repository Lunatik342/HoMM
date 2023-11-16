using System;
using System.Collections.Generic;
using Battle.Units;

namespace Battle.BattleFlow
{
    public class InitiativeSortedUnitsList
    {
        private List<Unit> _unitsQueue = new();
        
        public IReadOnlyList<Unit> SourceList => _unitsQueue;
        public event Action<Unit, int> UnitAdded; 
        public event Action<Unit, int> UnitRemoved;

        public void AddUnitToQueue(Unit unit)
        {
            var index = AddNewUnit(unit);
            UnitAdded?.Invoke(unit, index);
        }

        public void RemoveUnit(Unit unit)
        {
            var index = _unitsQueue.IndexOf(unit);

            if (index != -1)
            {
                _unitsQueue.RemoveAt(index);
                UnitRemoved?.Invoke(unit, index);
            }
        }

        private int AddNewUnit(Unit unit)
        {
            var initiative = unit.TurnsNotificationsReceiver.InitiativeWithRandomSpread;

            for (int i = 0; i < _unitsQueue.Count; i++)
            {
                var currentUnit = _unitsQueue[i];

                if (initiative > currentUnit.TurnsNotificationsReceiver.InitiativeWithRandomSpread)
                {
                    _unitsQueue.Insert(i, unit);
                    return i;
                }
            }
            
            _unitsQueue.Add(unit);
            return _unitsQueue.Count - 1;
        }
    }
}