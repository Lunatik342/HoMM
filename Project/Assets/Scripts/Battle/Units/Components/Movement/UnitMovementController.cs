using System.Collections.Generic;
using Algorithms.RogueSharp;
using Battle.CellViewsGrid.PathDisplay;
using Battle.StatsSystem;
using Battle.Units.Components.Interfaces;
using Battle.Units.StaticData.Components.Movement;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Battle.Units.Components.Movement
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