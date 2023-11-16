using Battle.Arena.ArenaView;
using Battle.Arena.Map;
using Battle.Arena.Obstacles;
using Battle.CellViewsGrid.CellsViews;
using Battle.CellViewsGrid.GridViewStateMachine;
using Battle.CellViewsGrid.GridViewStateMachine.States;
using Battle.Result;
using Battle.Units.Creation;
using Cysharp.Threading.Tasks;
using Infrastructure.SimpleStateMachine;
using UI.Hud;
using UI.LoadingScreen;
using UISystem;
using UnityEngine;

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
        
        private readonly UnitsQueueService _unitsQueueService;
        private readonly BattleFlowController _battleFlowController;
        private readonly GridViewStateMachine _gridViewStateMachine;
        private readonly StatesFactory _statesFactory;
        private readonly BattleResultEvaluator _battleResultEvaluator;
        private readonly BattlePhasesStateMachine _battlePhasesStateMachine;
        private readonly UIWindowsManager _uiWindowsManager;

        public BattleStartPhase(BattleFieldViewSpawner battleFieldViewSpawner,
            ObstaclesSpawner obstaclesSpawner,
            ArmySpawner armySpawner,
            BattleMapCreator battleMapCreator,
            BattleArenaCellsViewsSpawner cellsViewsSpawner,
            IObstaclesGenerationStrategy.Factory obstacleGenerationStrategyFactory,
            UnitsQueueService unitsQueueService,
            BattleFlowController battleFlowController,
            GridViewStateMachine gridViewStateMachine,
            StatesFactory statesFactory,
            BattleResultEvaluator battleResultEvaluator,
            BattlePhasesStateMachine battlePhasesStateMachine,
            UIWindowsManager uiWindowsManager)
        {
            _battleFieldViewSpawner = battleFieldViewSpawner;
            _obstaclesSpawner = obstaclesSpawner;
            _armySpawner = armySpawner;
            _battleMapCreator = battleMapCreator;
            _cellsViewsSpawner = cellsViewsSpawner;
            _obstacleGenerationStrategyFactory = obstacleGenerationStrategyFactory;
            _unitsQueueService = unitsQueueService;
            _battleFlowController = battleFlowController;
            _gridViewStateMachine = gridViewStateMachine;
            _statesFactory = statesFactory;
            _battleResultEvaluator = battleResultEvaluator;
            _battlePhasesStateMachine = battlePhasesStateMachine;
            _uiWindowsManager = uiWindowsManager;
        }

        public void Enter(BattleStartParameters battleStartParameters)
        {
            InitializeBattleSystems(battleStartParameters).Forget(Debug.LogError);
        }

        private async UniTask InitializeBattleSystems(BattleStartParameters battleStartParameters)
        {
            await CreateBattleField(battleStartParameters);
            await CreateArmies(battleStartParameters);
            InitializeGridViewStateMachine();
            InitializeBattleFlowSystems(battleStartParameters);
            await _uiWindowsManager.CloseWindow<LoadingWindow>();
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
            _unitsQueueService.AddAllAliveUnitsToQueue();
            _uiWindowsManager.OpenWindow<UnitsQueueWindow>(window => {window.PassParameters(_unitsQueueService);}).Forget(Debug.LogError);
            _battleResultEvaluator.SetParticipatingTeams(battleStartParameters.StartingUnits);
            _battleFlowController.CreateCommandProviders(battleStartParameters.CommandProvidersForTeams);
        }

        public void Exit()
        {
            
        }
    }
}