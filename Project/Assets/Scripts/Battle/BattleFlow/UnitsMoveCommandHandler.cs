using Battle.BattleArena.Pathfinding;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Battle.Units.Movement
{
    public class UnitsMoveCommandHandler
    {
        public async UniTask MakeMove(Unit unit, Vector2Int gridPosition)
        {
            await unit.MovementController.MoveToPosition(gridPosition);
        }
    }
}