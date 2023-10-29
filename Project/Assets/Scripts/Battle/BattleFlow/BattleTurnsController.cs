using System;
using System.Collections.Generic;
using Battle.BattleFlow.Commands;
using Battle.BattleFlow.StateMachine;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Battle.BattleFlow
{
    public class BattleTurnsController
    {
        private readonly TurnsQueueService _turnsQueueService;
        private readonly LocalPlayerControlledCommandProvider.Factory _localPlayerCommandProviderFactory;
        private readonly AIControlledCommandProvider.Factory _aiCommandProviderFactory;
        private readonly GridViewStateMachine _gridViewStateMachine;
        private readonly CommandsProcessor _commandsProcessor;
        private readonly GameResultEvaluator _gameResultEvaluator;
        private readonly ZenjectSceneLoader _sceneLoader;

        private readonly Dictionary<Team, ICommandProvider> _commandProviders = new();

        public BattleTurnsController(TurnsQueueService turnsQueueService, 
            LocalPlayerControlledCommandProvider.Factory localPlayerCommandProviderFactory, 
            AIControlledCommandProvider.Factory aiCommandProviderFactory,
            GridViewStateMachine gridViewStateMachine,
            CommandsProcessor commandsProcessor,
            GameResultEvaluator gameResultEvaluator,
            ZenjectSceneLoader sceneLoader)
        {
            _turnsQueueService = turnsQueueService;
            _localPlayerCommandProviderFactory = localPlayerCommandProviderFactory;
            _aiCommandProviderFactory = aiCommandProviderFactory;
            _gridViewStateMachine = gridViewStateMachine;
            _commandsProcessor = commandsProcessor;
            _gameResultEvaluator = gameResultEvaluator;
            _sceneLoader = sceneLoader;
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
                
                if (_gameResultEvaluator.IsGameOver(out var winningTeam))
                {
                    Debug.LogError("Game over");
                    //_sceneLoader.LoadScene(0);
                    break;
                }
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