using System;
using System.Collections.Generic;
using Battle.BattleFlow.Phases;
using Battle.CellViewsGrid.GridViewStateMachine;
using Battle.CellViewsGrid.GridViewStateMachine.States;
using Battle.UnitCommands.Processors;
using Battle.UnitCommands.Providers;
using Cysharp.Threading.Tasks;

namespace Battle.BattleFlow
{
    public class BattleTurnsController
    {
        private readonly UnitsQueueService _unitsQueueService;
        private readonly LocalPlayerControlledCommandProvider.Factory _localPlayerCommandProviderFactory;
        private readonly AIControlledCommandProvider.Factory _aiCommandProviderFactory;
        private readonly GridViewStateMachine _gridViewStateMachine;
        private readonly CommandsProcessorFacade _commandsProcessorFacade;
        private readonly GameResultEvaluator _gameResultEvaluator;
        private readonly BattlePhasesStateMachine _battlePhasesStateMachine;

        private readonly Dictionary<Team, ICommandProvider> _commandProviders = new();
        private readonly int _betweenTurnsDelay = 250;

        public BattleTurnsController(UnitsQueueService unitsQueueService, 
            LocalPlayerControlledCommandProvider.Factory localPlayerCommandProviderFactory, 
            AIControlledCommandProvider.Factory aiCommandProviderFactory,
            GridViewStateMachine gridViewStateMachine,
            CommandsProcessorFacade commandsProcessorFacade,
            GameResultEvaluator gameResultEvaluator,
            BattlePhasesStateMachine battlePhasesStateMachine)
        {
            _unitsQueueService = unitsQueueService;
            _localPlayerCommandProviderFactory = localPlayerCommandProviderFactory;
            _aiCommandProviderFactory = aiCommandProviderFactory;
            _gridViewStateMachine = gridViewStateMachine;
            _commandsProcessorFacade = commandsProcessorFacade;
            _gameResultEvaluator = gameResultEvaluator;
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

        public async void StartTurns()
        {
            while (true)
            {
                await MakeTurn();
                
                if (_gameResultEvaluator.IsGameOver(out _))
                {
                    _battlePhasesStateMachine.Enter<BattleEndPhase>();
                    break;
                }
            }
        }
        
        private async UniTask MakeTurn()
        {
            var targetUnit = _unitsQueueService.Dequeue();
            var command = await _commandProviders[targetUnit.Team].WaitForCommand(targetUnit);
            _gridViewStateMachine.Enter<WaitingForCommandProcessViewState>();
            await command.Process(_commandsProcessorFacade);
            await UniTask.Delay(_betweenTurnsDelay);
        }
    }
}