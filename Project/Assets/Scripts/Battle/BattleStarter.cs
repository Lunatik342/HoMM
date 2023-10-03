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
        private readonly ObstaclesSpawner _obstaclesSpawner;
        private readonly PathfindingService _pathfindingService;

        public BattleStarter(BattleFieldViewSpawner battleFieldViewSpawner,
            ObstaclesSpawner obstaclesSpawner,
            PathfindingService pathfindingService)
        {
            _battleFieldViewSpawner = battleFieldViewSpawner;
            _obstaclesSpawner = obstaclesSpawner;
            _pathfindingService = pathfindingService;
        }

        public async void Initialize()
        {
            await StartBattle();
        }

        private async Task StartBattle()
        {
            _obstaclesSpawner.SpawnRandomObstacles();
            _pathfindingService.Test();
            
            await _battleFieldViewSpawner.SpawnBattleField();
        }
    }
}