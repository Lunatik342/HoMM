using Battle.BattleArena;
using Battle.BattleArena.CellsViews;
using Battle.BattleArena.Obstacles;
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
        private readonly ArmySpawner _armySpawner;
        private readonly BattleMapCreator _battleMapCreator;
        private readonly BattleStartParameters _battleStartParameters;
        private readonly BattleArenaCellsViewsSpawner _cellsViewsSpawner;
        private readonly IObstaclesGenerationStrategy.Factory _obstacleGenerationStrategyFactory;
        private readonly TurnsQueueService _turnsQueueService;
        private readonly BattleTurnsController _battleTurnsController;

        public BattleStarter(BattleFieldViewSpawner battleFieldViewSpawner,
            ObstaclesSpawner obstaclesSpawner,
            ArmySpawner armySpawner,
            BattleMapCreator battleMapCreator,
            BattleStartParameters battleStartParameters,
            BattleArenaCellsViewsSpawner cellsViewsSpawner,
            IObstaclesGenerationStrategy.Factory obstacleGenerationStrategyFactory,
            TurnsQueueService turnsQueueService,
            BattleTurnsController battleTurnsController)
        {
            _battleFieldViewSpawner = battleFieldViewSpawner;
            _obstaclesSpawner = obstaclesSpawner;
            _armySpawner = armySpawner;
            _battleMapCreator = battleMapCreator;
            _battleStartParameters = battleStartParameters;
            _cellsViewsSpawner = cellsViewsSpawner;
            _obstacleGenerationStrategyFactory = obstacleGenerationStrategyFactory;
            _turnsQueueService = turnsQueueService;
            _battleTurnsController = battleTurnsController;
        }

        public async void Initialize()
        {
            await StartBattle();
        }

        private async Task StartBattle()
        {
            _battleMapCreator.Create(_battleStartParameters.BattleArenaId);
            _cellsViewsSpawner.Spawn(_battleStartParameters.BattleArenaId);

            var obstacleGenerationStrategy = _obstacleGenerationStrategyFactory.Create(
                _battleStartParameters.ObstacleGenerationParameters, _battleStartParameters.BattleArenaId);

            await _battleFieldViewSpawner.Spawn(_battleStartParameters.BattleArenaId);
            await _obstaclesSpawner.Spawn(obstacleGenerationStrategy);
            await _armySpawner.Spawn(_battleStartParameters.StartingUnits);
            _turnsQueueService.InitializeFromStartingUnits();
            _battleTurnsController.StartTurns();
        }
    }
}