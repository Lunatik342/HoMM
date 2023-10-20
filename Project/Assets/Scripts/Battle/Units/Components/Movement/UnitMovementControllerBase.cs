using Battle.Units.StatsSystem;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Battle.Units.Movement
{
    public abstract class UnitMovementControllerBase: IUnitInitializable
    {
        private readonly MovementStaticData _movementStaticData;
        private readonly UnitStatsProvider _statsProviderProvider;

        public int TravelDistance => _statsProviderProvider.GetStatValue(StatType.TravelDistance);

        protected UnitMovementControllerBase(MovementStaticData movementStaticData, 
            UnitStatsProvider statsProviderProvider)
        {
            _movementStaticData = movementStaticData;
            _statsProviderProvider = statsProviderProvider;
        }

        void IUnitInitializable.Initialize()
        {
            _statsProviderProvider.AddStat(StatType.TravelDistance, _movementStaticData.TravelDistance);
        }

        public abstract UniTask MoveToPosition(Vector2Int targetPosition);
    }
}