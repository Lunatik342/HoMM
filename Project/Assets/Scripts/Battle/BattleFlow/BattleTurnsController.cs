using System;
using System.Collections.Generic;
using Battle.BattleFlow.Commands;
using Battle.BattleFlow.StateMachine;
using Cysharp.Threading.Tasks;

namespace Battle.BattleFlow
{
    public class BattleTurnsController
    {
        private readonly TurnsQueueService _turnsQueueService;
        private readonly LocalPlayerControlledCommandProvider.Factory _localPlayerCommandProviderFactory;
        private readonly AIControlledCommandProvider.Factory _aiCommandProviderFactory;
        private readonly GridViewStateMachine _gridViewStateMachine;
        private readonly CommandsProcessor _commandsProcessor;

        private readonly Dictionary<Team, ICommandProvider> _commandProviders = new();

        public BattleTurnsController(TurnsQueueService turnsQueueService, 
            LocalPlayerControlledCommandProvider.Factory localPlayerCommandProviderFactory, 
            AIControlledCommandProvider.Factory aiCommandProviderFactory,
            GridViewStateMachine gridViewStateMachine,
            CommandsProcessor commandsProcessor)
        {
            _turnsQueueService = turnsQueueService;
            _localPlayerCommandProviderFactory = localPlayerCommandProviderFactory;
            _aiCommandProviderFactory = aiCommandProviderFactory;
            _gridViewStateMachine = gridViewStateMachine;
            _commandsProcessor = commandsProcessor;
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
            }
        }
        
        private async UniTask MakeTurn()
        {
            var targetUnit = _turnsQueueService.Dequeue();
            var command = await _commandProviders[targetUnit.Team].WaitForCommand(targetUnit);
            _gridViewStateMachine.Enter<WaitingForCommandProcessViewState>();
            await command.Process(_commandsProcessor);
            await UniTask.Delay(250);
        }
    }
}