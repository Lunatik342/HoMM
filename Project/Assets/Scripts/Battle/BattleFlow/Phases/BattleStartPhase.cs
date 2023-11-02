using Battle.BattleArena;
using Battle.BattleArena.CellsViews;
using Battle.BattleArena.Obstacles;
using Battle.BattleArena.Pathfinding;
using Battle.BattleFlow.StateMachine;
using Battle.Units;
using Cysharp.Threading.Tasks;
using Infrastructure.SimpleStateMachine;

namespace Battle.BattleFlow.Phases
{
    public class BattleStartPhase: IPaylodedState<BattleStartParameters>
    {
        private readonly BattleFieldViewSpawner _battleFieldViewSpawner;
        private readonly ObstaclesSpawner _obstaclesSpawner;
        private readonly ArmySpawner _armySpawner;
        private readonly BattleMapCreator _battleMapCreator;
        private readonly BattleArenaCellsViewsSpawner _cellsViewsSpawner;
        private readonly IObstaclesGenerationStrategy.Factory _obstacleGenerationStrategyFactory;
        
        private readonly TurnsQueueService _turnsQueueService;
        private readonly BattleTurnsController _battleTurnsController;
        private readonly GridViewStateMachine _gridViewStateMachine;
        private readonly StatesFactory _statesFactory;
        private readonly GameResultEvaluator _gameResultEvaluator;
        private readonly BattlePhasesStateMachine _battlePhasesStateMachine;

        public BattleStartPhase(BattleFieldViewSpawner battleFieldViewSpawner,
            ObstaclesSpawner obstaclesSpawner,
            ArmySpawner armySpawner,
            BattleMapCreator battleMapCreator,
            BattleArenaCellsViewsSpawner cellsViewsSpawner,
            IObstaclesGenerationStrategy.Factory obstacleGenerationStrategyFactory,
            TurnsQueueService turnsQueueService,
            BattleTurnsController battleTurnsController,
            GridViewStateMachine gridViewStateMachine,
            StatesFactory statesFactory,
            GameResultEvaluator gameResultEvaluator,
            BattlePhasesStateMachine battlePhasesStateMachine)
        {
            _battleFieldViewSpawner = battleFieldViewSpawner;
            _obstaclesSpawner = obstaclesSpawner;
            _armySpawner = armySpawner;
            _battleMapCreator = battleMapCreator;
            _cellsViewsSpawner = cellsViewsSpawner;
            _obstacleGenerationStrategyFactory = obstacleGenerationStrategyFactory;
            _turnsQueueService = turnsQueueService;
            _battleTurnsController = battleTurnsController;
            _gridViewStateMachine = gridViewStateMachine;
            _statesFactory = statesFactory;
            _gameResultEvaluator = gameResultEvaluator;
            _battlePhasesStateMachine = battlePhasesStateMachine;
        }

        public void Enter(BattleStartParameters battleStartParameters)
        {
            InitializeBattleSystems(battleStartParameters).Forget();
        }

        private async UniTaskVoid InitializeBattleSystems(BattleStartParameters battleStartParameters)
        {
            await CreateBattleField(battleStartParameters);
            await CreateArmies(battleStartParameters);
            InitializeGridViewStateMachine();
            InitializeBattleFlowSystems(battleStartParameters);
            _battlePhasesStateMachine.Enter<BattleProgressPhase>();
        }

        private async UniTask CreateBattleField(BattleStartParameters battleStartParameters)
        {
            _battleMapCreator.Create(battleStartParameters.BattleArenaId);
            _cellsViewsSpawner.Spawn(battleStartParameters.BattleArenaId);

            var obstacleGenerationStrategy = _obstacleGenerationStrategyFactory.Create(
                battleStartParameters.ObstacleGenerationParameters, battleStartParameters.BattleArenaId);

            await UniTask.WhenAll(
                _battleFieldViewSpawner.Spawn(battleStartParameters.BattleArenaId), 
                _obstaclesSpawner.Spawn(obstacleGenerationStrategy));
        }

        private async UniTask CreateArmies(BattleStartParameters battleStartParameters)
        {
            await _armySpawner.Spawn(battleStartParameters.StartingUnits);
        }

        private void InitializeGridViewStateMachine()
        {
            _gridViewStateMachine.RegisterState(_statesFactory.Create<UnitControlViewState>());
            _gridViewStateMachine.RegisterState(_statesFactory.Create<WaitingForCommandProcessViewState>());
            _gridViewStateMachine.RegisterState(_statesFactory.Create<WaitingForEnemyTurnViewState>());
        }

        private void InitializeBattleFlowSystems(BattleStartParameters battleStartParameters)
        {
            _turnsQueueService.InitializeFromStartingUnits();
            _gameResultEvaluator.SetParticipatingTeams(battleStartParameters.StartingUnits);
            _battleTurnsController.CreateCommandProviders(battleStartParameters.CommandProvidersForTeams);
        }

        public void Exit()
        {
            
        }
    }
}