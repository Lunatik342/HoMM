using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;

namespace Battle.Units.Creation
{
    public class ArmySpawner: IUnitsHolder
    {
        private readonly UnitSpawner _unitSpawner;

        private Dictionary<Team, List<Unit>> _unitsOfTeam = new();

        public ArmySpawner(UnitSpawner unitSpawner)
        {
            _unitSpawner = unitSpawner;
        }

        public async UniTask Spawn(Dictionary<Team, List<UnitCreationParameter>> startingUnits)
        {
            List<UniTask> unitCreationTasks = new List<UniTask>();

            foreach (var startingUnitsOfTeam in startingUnits)
            {
                var createdUnitsOfTeam = new List<Unit>();
                
                foreach (var unitCreationParameter in startingUnitsOfTeam.Value)
                {
                    unitCreationTasks.Add(CreateUnit(unitCreationParameter, startingUnitsOfTeam.Key, createdUnitsOfTeam));
                }

                _unitsOfTeam[startingUnitsOfTeam.Key] = createdUnitsOfTeam;
            }

            await UniTask.WhenAll(unitCreationTasks);
        }

        public IEnumerable<Unit> GetAllUnits()
        {
            foreach (var teamUnits in _unitsOfTeam)
            {
                foreach (var unit in teamUnits.Value)
                {
                    yield return unit;
                }
            }
        }

        public List<Unit> GetAllUnitsOfTeam(Team team)
        {
            return _unitsOfTeam[team];
        }

        public List<Unit> GetAllAliveUnitsOfTeam(Team team)
        {
            return _unitsOfTeam[team].Where(u => u.Health.IsAlive).ToList();
        }

        private async UniTask CreateUnit(UnitCreationParameter unitCreationParameter, Team team, List<Unit> holder)
        {
            var unit = await _unitSpawner.Create(unitCreationParameter, team);
            holder.Add(unit);
        }
    }
}