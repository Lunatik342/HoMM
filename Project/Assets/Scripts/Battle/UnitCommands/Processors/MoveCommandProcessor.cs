using Battle.Units;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Battle.UnitCommands.Processors
{
    public class MoveCommandProcessor
    {
        public async UniTask Process(Unit unit, Vector2Int gridPosition)
        {
            await unit.MovementController.MoveToPosition(gridPosition);
            await unit.RotationController.SmoothLookAtEnemySide();
        }
    }
}