using Cysharp.Threading.Tasks;

namespace Battle.BattleFlow
{
    public class BattleTurnsProcessor
    {
        private readonly TurnsQueueService _turnsQueueService;
        private readonly TurnControllerLocal _turnControllerLocal;

        public BattleTurnsProcessor(TurnsQueueService turnsQueueService, TurnControllerLocal turnControllerLocal)
        {
            _turnsQueueService = turnsQueueService;
            _turnControllerLocal = turnControllerLocal;
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
            await _turnControllerLocal.WaitForCommand(targetUnit);
        }
    }
}