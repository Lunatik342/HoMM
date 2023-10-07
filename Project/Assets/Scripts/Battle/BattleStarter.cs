using Battle.BattleArena;
using Battle.BattleArena.Pathfinding;
using Battle.BattleArena.Pathfinding.StaticData;
using Battle.BattleFlow;
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
        private readonly UnitsFactory _unitsFactory;
        private readonly UnitsMoveCommandHandler _moveCommandHandler;
        private readonly BattleCommandsDispatcher _battleCommandsDispatcher;

        public BattleStarter(BattleFieldViewSpawner battleFieldViewSpawner,
            ObstaclesSpawner obstaclesSpawner,
            UnitsFactory unitsFactory,
            UnitsMoveCommandHandler moveCommandHandler,
            BattleCommandsDispatcher battleCommandsDispatcher)
        {
            _battleFieldViewSpawner = battleFieldViewSpawner;
            _obstaclesSpawner = obstaclesSpawner;
            _unitsFactory = unitsFactory;
            _moveCommandHandler = moveCommandHandler;
            _battleCommandsDispatcher = battleCommandsDispatcher;
        }

        public async void Initialize()
        {
            await StartBattle();
        }

        private async Task StartBattle()
        {
            await _obstaclesSpawner.SpawnObstacles();
            var unit = await _unitsFactory.Create(UnitId.Blank, new Vector2Int(0, 2));
            await _battleFieldViewSpawner.SpawnBattleField();

            //await _moveCommandHandler.MakeMove(unit, new Vector2Int(9, 9));
            
            _battleCommandsDispatcher.IsEnabled = true;
        }
    }
}