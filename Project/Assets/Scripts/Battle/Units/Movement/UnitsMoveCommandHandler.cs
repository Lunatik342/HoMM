using System;
using Battle.BattleArena.Pathfinding;
using Battle.BattleArena.Pathfinding.Movement;
using Cysharp.Threading.Tasks;
using RogueSharp;
using UnityEngine;

namespace Battle.Units.Movement
{
    public class UnitsMoveCommandHandler
    {
        private readonly Map _map;
        private readonly PathfindingService _pathfindingService;

        public UnitsMoveCommandHandler(Map map, PathfindingService pathfindingService)
        {
            _map = map;
            _pathfindingService = pathfindingService;
        }
        
        public async UniTask MakeMove(Unit unit, Vector2Int gridPosition)
        {
            switch (unit.MovementType)
            {
                case MovementType.None:
                    Debug.LogError("Cannot move stationary character");
                    break;
                case MovementType.OnGround:
                    var movementController = new OnGroundMapMovementHandler(_map, _pathfindingService);
                    await movementController.MoveToPosition(gridPosition, unit);
                    break;
                case MovementType.Flying:
                    throw new NotImplementedException();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}