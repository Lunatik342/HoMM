using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Battle.Units.Movement
{
    public interface IUnitMovementController
    {
        UniTask MoveToPosition(Vector2Int mapPosition);
    }
}