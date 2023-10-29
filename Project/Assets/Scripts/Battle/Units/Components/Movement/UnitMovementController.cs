using System.Collections.Generic;
using Battle.BattleArena.PathDisplay;
using Battle.Units.StatsSystem;
using Cysharp.Threading.Tasks;
using RogueSharp;
using UnityEngine;

namespace Battle.Units.Movement
{
    public abstract class UnitMovementController: IStatsInitializer
    {
        private readonly MovementStaticData _movementStaticData;
        private readonly UnitStatsProvider _statsProviderProvider;

        protected UnitStat _travelDistanceStat;

        protected UnitMovementController(MovementStaticData movementStaticData, 
            UnitStatsProvider statsProviderProvider)
        {
            _movementStaticData = movementStaticData;
            _statsProviderProvider = statsProviderProvider;
        }

        void IStatsInitializer.ConfigureStats()
        {
            _travelDistanceStat = _statsProviderProvider.AddStat(StatType.TravelDistance, _movementStaticData.TravelDistance);
        }

        public abstract List<Cell> GetReachableCells();
        public abstract void DisplayPathToCell(PathDisplayService pathDisplayService, Vector2Int position);
        public abstract UniTask MoveToPosition(Vector2Int targetPosition);
    }
}