using Battle.BattleFlow.Commands;
using Battle.BattleFlow.StateMachine;
using Cysharp.Threading.Tasks;

namespace Battle.BattleFlow
{
    public class BattleTurnsController
    {
        private readonly TurnsQueueService _turnsQueueService;
        private readonly LocalPlayerCommandProvider _localPlayerCommandProvider;
        private readonly GridViewStateMachine _gridViewStateMachine;
        private readonly CommandsProcessor _commandsProcessor;

        public BattleTurnsController(TurnsQueueService turnsQueueService, 
            LocalPlayerCommandProvider localPlayerCommandProvider, 
            GridViewStateMachine gridViewStateMachine,
            CommandsProcessor commandsProcessor)
        {
            _turnsQueueService = turnsQueueService;
            _localPlayerCommandProvider = localPlayerCommandProvider;
            _gridViewStateMachine = gridViewStateMachine;
            _commandsProcessor = commandsProcessor;
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
            var command = await _localPlayerCommandProvider.WaitForCommand(targetUnit);
            _gridViewStateMachine.Enter<WaitingForCommandProcessViewState>();
            await command.Process(_commandsProcessor);
            await UniTask.Delay(250);
        }
    }
}