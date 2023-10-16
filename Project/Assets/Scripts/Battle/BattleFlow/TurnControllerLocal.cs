using Battle.BattleArena.Pathfinding;
using Cysharp.Threading.Tasks;

namespace Battle.BattleFlow
{
    public class TurnControllerLocal
    {
        private readonly UnitOnGridController _unitOnGridController;

        public TurnControllerLocal(UnitOnGridController unitOnGridController)
        {
            _unitOnGridController = unitOnGridController;
        }

        public async UniTask WaitForCommand(Unit unit)
        {
            await _unitOnGridController.Do(unit);
            _unitOnGridController.StopDoing();
        }
    }
}