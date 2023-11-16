using System;
using System.Collections.Generic;
using Battle.Units;
using Battle.Units.Creation;

namespace Battle.BattleFlow
{
    public class UnitsQueueService
    {
        private readonly IUnitsHolder _unitsHolder;
        private readonly InitiativeSortedList _currentTurnQueue;

        public InitiativeSortedList CurrentTurnQueue => _currentTurnQueue;
        public int CurrentTurn = 1;

        public UnitsQueueService(IUnitsHolder unitsHolder)
        {
            _unitsHolder = unitsHolder;
            _currentTurnQueue = new InitiativeSortedList();
        }

        public void AddAllAliveUnitsToQueue()
        {
            var allUnits = _unitsHolder.GetAllUnits();

            foreach (var unit in allUnits)
            {
                if (unit.Health.IsAlive)
                {
                    _currentTurnQueue.AddUnitToQueue(unit);
                    unit.Health.UnitDied += RemoveUnitFromQueue;
                }
            }
        }

        public Unit GetNextUnitInQueue()
        {
            return _currentTurnQueue.SourceList[0];
        }

        public bool StartNewTurnIfNeeded()
        {
            if (_currentTurnQueue.SourceList.Count == 0)
            {
                AddAllAliveUnitsToQueue();

                foreach (var unit in _currentTurnQueue.SourceList)
                {
                    unit.TurnsHelper.NotifyTurnEnd();
                }
                
                CurrentTurn++;
                return true;
            }

            return false;
        }

        public void RemoveUnitFromQueue(Unit diedUnit)
        {
            _currentTurnQueue.RemoveUnit(diedUnit);
            diedUnit.Health.UnitDied -= RemoveUnitFromQueue;
        }
    }

    public class InitiativeSortedList
    {
        private List<Unit> _unitsQueue = new List<Unit>();
        
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
            var initiative = unit.TurnsHelper.InitiativeWithRandomSpread;

            for (int i = 0; i < _unitsQueue.Count; i++)
            {
                var currentUnit = _unitsQueue[i];

                if (initiative > currentUnit.TurnsHelper.InitiativeWithRandomSpread)
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