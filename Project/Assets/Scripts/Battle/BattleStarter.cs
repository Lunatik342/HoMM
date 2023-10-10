using Battle.BattleArena;
using Battle.BattleArena.Pathfinding;
using Battle.BattleArena.Pathfinding.StaticData;
using Battle.BattleFlow;
using Battle.Units;
using Battle.Units.Movement;
using UnityEngine;
using Zenject;
using Task = System.Threading.Tasks.Task;

namespace Battle
{
    public class BattleStarter: IInitializable
    {
        private readonly BattleFieldViewSpawner _battleFieldViewSpawner;
        private readonly ObstaclesSpawner _obstaclesSpawner;
        private readonly BattleCommandsDispatcher _battleCommandsDispatcher;
        private readonly UnitsFactory _unitsFactory;

        public BattleStarter(BattleFieldViewSpawner battleFieldViewSpawner,
            ObstaclesSpawner obstaclesSpawner,
            BattleCommandsDispatcher battleCommandsDispatcher,
            UnitsFactory unitsFactory)
        {
            _battleFieldViewSpawner = battleFieldViewSpawner;
            _obstaclesSpawner = obstaclesSpawner;
            _battleCommandsDispatcher = battleCommandsDispatcher;
            _unitsFactory = unitsFactory;
        }

        public async void Initialize()
        {
            await StartBattle();
        }

        private async Task StartBattle()
        {
            await _obstaclesSpawner.SpawnObstacles();
            await _unitsFactory.Create(UnitId.Blank, new Vector2Int(0, 2));
            await _battleFieldViewSpawner.SpawnBattleField();
            
            _battleCommandsDispatcher.IsEnabled = true;
        }
    }
}