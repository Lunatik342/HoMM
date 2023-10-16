using System.Collections.Generic;
using System.Threading.Tasks;
using Battle.BattleArena.Pathfinding;
using Cysharp.Threading.Tasks;

namespace Battle.Units
{
    public class ArmySpawner: IUnitsHolder
    {
        private readonly UnitSpawner _unitSpawner;

        public List<Unit> AllUnits { get; } = new();

        public ArmySpawner(UnitSpawner unitSpawner)
        {
            _unitSpawner = unitSpawner;
        }

        public async Task Spawn(Dictionary<Team, List<UnitCreationParameter>> startingUnits)
        {
            List<UniTask> unitCreationTasks = new List<UniTask>();

            foreach (var unitsOfTeam in startingUnits)
            {
                foreach (var unitCreationParameter in unitsOfTeam.Value)
                {
                    unitCreationTasks.Add(CreateUnit(unitCreationParameter, unitsOfTeam.Key));
                }
            }

            await UniTask.WhenAll(unitCreationTasks);
        }

        private async UniTask CreateUnit(UnitCreationParameter unitCreationParameter, Team team)
        {
            var unit = await _unitSpawner.Create(unitCreationParameter, team);
            AllUnits.Add(unit);
        }
    }
}