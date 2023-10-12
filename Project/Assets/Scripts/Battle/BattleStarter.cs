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
        private readonly BattleCommandsDispatcher _battleCommandsDispatcher;
        private readonly UnitsSpawner _unitsSpawner;
        private readonly BattleMapCreator _battleMapCreator;
        private readonly BattleStartParameters _battleStartParameters;
        private readonly BattleArenaCellsViewsSpawner _cellsViewsSpawner;
        private readonly IObstaclesGenerationStrategy.Factory _obstacleGenerationStrategyFactory;

        public BattleStarter(BattleFieldViewSpawner battleFieldViewSpawner,
            ObstaclesSpawner obstaclesSpawner,
            BattleCommandsDispatcher battleCommandsDispatcher,
            UnitsSpawner unitsSpawner,
            BattleMapCreator battleMapCreator,
            BattleStartParameters battleStartParameters,
            BattleArenaCellsViewsSpawner cellsViewsSpawner,
            IObstaclesGenerationStrategy.Factory obstacleGenerationStrategyFactory)
        {
            _battleFieldViewSpawner = battleFieldViewSpawner;
            _obstaclesSpawner = obstaclesSpawner;
            _battleCommandsDispatcher = battleCommandsDispatcher;
            _unitsSpawner = unitsSpawner;
            _battleMapCreator = battleMapCreator;
            _battleStartParameters = battleStartParameters;
            _cellsViewsSpawner = cellsViewsSpawner;
            _obstacleGenerationStrategyFactory = obstacleGenerationStrategyFactory;
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
            await _unitsSpawner.Create(UnitId.Blank, new Vector2Int(0, 2), Team.TeamLeft);

            _battleCommandsDispatcher.IsEnabled = true;
        }
    }
}