using System;
using System.Collections.Generic;
using Battle.BattleFlow.Phases;
using Battle.CellViewsGrid.GridViewStateMachine;
using Battle.CellViewsGrid.GridViewStateMachine.States;
using Battle.Result;
using Battle.UnitCommands.Processors;
using Battle.UnitCommands.Providers;
using Cysharp.Threading.Tasks;

namespace Battle.BattleFlow
{
    public class BattleFlowController
    {
        private readonly UnitsQueueService _unitsQueueService;
        private readonly LocalPlayerControlledCommandProvider.Factory _localPlayerCommandProviderFactory;
        private readonly AIControlledCommandProvider.Factory _aiCommandProviderFactory;
        private readonly GridViewStateMachine _gridViewStateMachine;
        private readonly CommandsProcessorFacade _commandsProcessorFacade;
        private readonly BattleResultEvaluator _battleResultEvaluator;
        private readonly BattlePhasesStateMachine _battlePhasesStateMachine;

        private readonly Dictionary<Team, ICommandProvider> _commandProviders = new();
        private readonly int _betweenTurnsDelay = 250;

        public BattleFlowController(UnitsQueueService unitsQueueService, 
            LocalPlayerControlledCommandProvider.Factory localPlayerCommandProviderFactory, 
            AIControlledCommandProvider.Factory aiCommandProviderFactory,
            GridViewStateMachine gridViewStateMachine,
            CommandsProcessorFacade commandsProcessorFacade,
            BattleResultEvaluator battleResultEvaluator,
            BattlePhasesStateMachine battlePhasesStateMachine)
        {
            _unitsQueueService = unitsQueueService;
            _localPlayerCommandProviderFactory = localPlayerCommandProviderFactory;
            _aiCommandProviderFactory = aiCommandProviderFactory;
            _gridViewStateMachine = gridViewStateMachine;
            _commandsProcessorFacade = commandsProcessorFacade;
            _battleResultEvaluator = battleResultEvaluator;
            _battlePhasesStateMachine = battlePhasesStateMachine;
        }

        public void CreateCommandProviders(Dictionary<Team, CommandProviderType> providersForTeams)
        {
            foreach (var providerForTeam in providersForTeams)
            {
                ICommandProvider commandProvider;
                
                switch (providerForTeam.Value)
                {
                    case CommandProviderType.PlayerControlled:
                        commandProvider = _localPlayerCommandProviderFactory.Create();
                        break;
                    case CommandProviderType.AIControlled:
                        commandProvider = _aiCommandProviderFactory.Create();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                _commandProviders[providerForTeam.Key] = commandProvider;
            }
        }

        public async UniTaskVoid StartBattleFlow()
        {
            while (true)
            {
                await MakeNextUnitMove();
                
                if (_battleResultEvaluator.IsGameOver(out _))
                {
                    _battlePhasesStateMachine.Enter<BattleEndPhase>();
                    break;
                }
                
                var newTurnStarted = _unitsQueueService.StartNewTurnIfNeeded();
            }
        }
        
        private async UniTask MakeNextUnitMove()
        {
            var targetUnit = _unitsQueueService.GetNextUnitInQueue();
            
            var command = await _commandProviders[targetUnit.Team].WaitForCommand(targetUnit);
            _gridViewStateMachine.Enter<WaitingForCommandProcessViewState>();
            await command.Process(_commandsProcessorFacade);
            
            _unitsQueueService.RemoveUnitFromQueue(targetUnit);
            await UniTask.Delay(_betweenTurnsDelay);
        }
    }
}