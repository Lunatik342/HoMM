using Battle.Units;
using Battle.Units.Creation;

namespace Battle.BattleFlow
{
    public class UnitsQueueService
    {
        private readonly IUnitsHolder _unitsHolder;
        private readonly InitiativeSortedUnitsList _currentTurnQueue;

        public InitiativeSortedUnitsList CurrentTurnQueue => _currentTurnQueue;

        public UnitsQueueService(IUnitsHolder unitsHolder)
        {
            _unitsHolder = unitsHolder;
            _currentTurnQueue = new InitiativeSortedUnitsList();
        }

        public void AddAllAliveUnitsToQueue()
        {
            var allUnits = _unitsHolder.GetAllAliveUnits();

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

        public void RemoveUnitFromQueue(Unit diedUnit)
        {
            _currentTurnQueue.RemoveUnit(diedUnit);
            diedUnit.Health.UnitDied -= RemoveUnitFromQueue;
        }

        public bool StartNewTurnIfNeeded()
        {
            if (_currentTurnQueue.SourceList.Count == 0)
            {
                AddAllAliveUnitsToQueue();
                NotifyUnitsAboutTurnEnd();
                
                return true;
            }

            return false;
        }

        private void NotifyUnitsAboutTurnEnd()
        {
            foreach (var unit in _currentTurnQueue.SourceList)
            {
                unit.TurnsNotificationsReceiver.NotifyTurnEnd();
            }
        }
    }
}