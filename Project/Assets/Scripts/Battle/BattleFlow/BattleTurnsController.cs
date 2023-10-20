using System.Collections.Generic;
using Battle.BattleFlow.Commands;
using Battle.BattleFlow.StateMachine;
using Cysharp.Threading.Tasks;

namespace Battle.BattleFlow
{
    public class BattleTurnsController
    {
        private readonly TurnsQueueService _turnsQueueService;
        private readonly GridViewStateMachine _gridViewStateMachine;
        private readonly CommandsProcessor _commandsProcessor;

        private Dictionary<Team, ICommandProvider> _commandProviders = new();

        public BattleTurnsController(TurnsQueueService turnsQueueService, 
            LocalPlayerCommandProvider localPlayerCommandProvider, 
            AICommandProvider aiCommandProvider,
            GridViewStateMachine gridViewStateMachine,
            CommandsProcessor commandsProcessor)
        {
            _turnsQueueService = turnsQueueService;
            _gridViewStateMachine = gridViewStateMachine;
            _commandsProcessor = commandsProcessor;

            _commandProviders[Team.TeamLeft] = localPlayerCommandProvider;
            _commandProviders[Team.TeamRight] = aiCommandProvider;
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