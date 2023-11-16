using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace Battle.Units.Creation
{
    public class ArmySpawner: IUnitsHolder
    {
        private readonly UnitSpawner _unitSpawner;

        private Dictionary<Team, List<Unit>> _unitsOfTeam = new();
        private Dictionary<Team, List<Unit>> _aliveUnitsOfTeam = new();

        public ArmySpawner(UnitSpawner unitSpawner)
        {
            _unitSpawner = unitSpawner;
        }

        public async UniTask Spawn(Dictionary<Team, List<UnitCreationParameter>> startingUnits)
        {
            List<UniTask> unitCreationTasks = new List<UniTask>();

            foreach (var startingUnitsOfTeam in startingUnits)
            {
                _unitsOfTeam[startingUnitsOfTeam.Key] = new List<Unit>();
                _aliveUnitsOfTeam[startingUnitsOfTeam.Key] = new List<Unit>();
                
                foreach (var unitCreationParameter in startingUnitsOfTeam.Value)
                {
                    unitCreationTasks.Add(CreateUnit(unitCreationParameter, startingUnitsOfTeam.Key));
                }
            }

            await UniTask.WhenAll(unitCreationTasks);

            SubscribeToDeathEvent();
        }

        IEnumerable<Unit> IUnitsHolder.GetAllUnits()
        {
            foreach (var teamUnits in _unitsOfTeam)
            {
                foreach (var unit in teamUnits.Value)
                {
                    yield return unit;
                }
            }
        }

        IEnumerable<Unit> IUnitsHolder.GetAllAliveUnits()
        {
            foreach (var teamUnits in _aliveUnitsOfTeam)
            {
                foreach (var unit in teamUnits.Value)
                {
                    yield return unit;
                }
            }
        }

        List<Unit> IUnitsHolder.GetAllUnitsOfTeam(Team team)
        {
            return _unitsOfTeam[team];
        }

        List<Unit> IUnitsHolder.GetAllAliveUnitsOfTeam(Team team)
        {
            return _aliveUnitsOfTeam[team];
        }

        private async UniTask CreateUnit(UnitCreationParameter unitCreationParameter, Team team)
        {
            var unit = await _unitSpawner.Create(unitCreationParameter, team);
            _aliveUnitsOfTeam[unit.Team].Add(unit);
            _unitsOfTeam[unit.Team].Add(unit);
        }

        private void SubscribeToDeathEvent()
        {
            foreach (var unit in ((IUnitsHolder)this).GetAllUnits())
            {
                unit.Health.UnitDied += RemoveFromAliveList;
            }
        }

        private void RemoveFromAliveList(Unit unit)
        {
            unit.Health.UnitDied -= RemoveFromAliveList;
            _aliveUnitsOfTeam[unit.Team].Remove(unit);
        }
    }
}