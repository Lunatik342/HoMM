using Battle.BattleArena;
using Battle.BattleArena.Pathfinding;
using Battle.BattleArena.StaticData;
using UnityEditor.VersionControl;
using Zenject;
using Task = System.Threading.Tasks.Task;

namespace Battle
{
    public class BattleStarter: IInitializable
    {
        private readonly BattleFieldViewSpawner _battleFieldViewSpawner;
        private readonly PathfindingMapFactory _pathfindingMapFactory;
        private readonly RandomObstaclesFactory _randomObstaclesFactory;
        private readonly PathfindingService _pathfindingService;

        public BattleStarter(BattleFieldViewSpawner battleFieldViewSpawner, 
            PathfindingMapFactory pathfindingMapFactory,
            RandomObstaclesFactory randomObstaclesFactory,
            PathfindingService pathfindingService)
        {
            _battleFieldViewSpawner = battleFieldViewSpawner;
            _pathfindingMapFactory = pathfindingMapFactory;
            _randomObstaclesFactory = randomObstaclesFactory;
            _pathfindingService = pathfindingService;
        }

        public async void Initialize()
        {
            await StartBattle();
        }

        private async Task StartBattle()
        {
            var targetBattleFieldId = BattleArenaId.Blank;
            
            _pathfindingMapFactory.CreatePathfindingGrid(targetBattleFieldId);
            _randomObstaclesFactory.SpawnRandomObstacles();
            _pathfindingService.Test();
            
            await _battleFieldViewSpawner.SpawnBattleField();
        }
    }
}