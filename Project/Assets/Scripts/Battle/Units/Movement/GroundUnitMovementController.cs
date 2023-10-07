using Battle.BattleArena;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Battle.Units.Movement
{
    public class GroundUnitMovementController: IUnitMovementController
    {
        private readonly GameObject _gameObject;
        private readonly RotationController _rotationController;

        public GroundUnitMovementController(GameObject gameObject, RotationController rotationController)
        {
            _gameObject = gameObject;
            _rotationController = rotationController;
        }
        
        public async UniTask MoveToPosition(Vector2Int mapPosition)
        {
            _rotationController.SmoothLookAt(mapPosition.ToBattleArenaWorldPosition()).Forget();
            await _gameObject.transform.DOMove(mapPosition.ToBattleArenaWorldPosition(), 0.25f).SetEase(Ease.Linear).ToUniTask();
        }
    }
}