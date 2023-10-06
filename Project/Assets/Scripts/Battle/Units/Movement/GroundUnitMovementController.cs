using Battle.BattleArena;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Battle.Units.Movement
{
    public class GroundUnitMovementController: IUnitMovementController
    {
        private readonly GameObject _gameObject;

        public GroundUnitMovementController(GameObject gameObject)
        {
            _gameObject = gameObject;
        }
        
        public async UniTask MoveToPosition(Vector2Int mapPosition)
        {
            await _gameObject.transform.DOMove(mapPosition.ToBattleArenaWorldPosition(), 0.5f).ToUniTask();
        }
    }
}