using System.Collections.Generic;
using Battle.BattleArena;
using Battle.BattleArena.Pathfinding;
using Battle.BattleArena.Pathfinding.StaticData;
using Cysharp.Threading.Tasks;
using Infrastructure.AssetManagement;
using RogueSharp;
using UnityEngine;
using Utilities;

namespace Battle.Units
{
    public class UnitsSpawner
    {
        private readonly Unit.Factory _unitsFactory;
        private readonly UnitsStaticDataProvider _unitsStaticDataProvider;
        private readonly AssetsLoadingService _assetsLoadingService;
        private readonly IMapHolder _mapHolder;

        public List<Unit> CreatedUnits = new();

        public UnitsSpawner(Unit.Factory unitsFactory, 
            UnitsStaticDataProvider unitsStaticDataProvider, 
            AssetsLoadingService assetsLoadingService,
            IMapHolder mapHolder)
        {
            _unitsFactory = unitsFactory;
            _unitsStaticDataProvider = unitsStaticDataProvider;
            _assetsLoadingService = assetsLoadingService;
            _mapHolder = mapHolder;
        }
        
        public async UniTask Create(UnitId unitId, Vector2Int position, Team team)
        {
            var unitStaticData = _unitsStaticDataProvider.ForUnit(unitId);
            
            var gameObject = await _assetsLoadingService.InstantiateAsync(unitStaticData.GameObjectAssetReference, 
                position.ToBattleArenaWorldPosition(), Quaternion.identity, null);
            
            var createdUnit = _unitsFactory.Create(gameObject.gameObject, team, unitStaticData);
            
            var size = createdUnit.BattleMapPlaceable.Size;
            createdUnit.GameObject.transform.position = TwoDimensionalArrayUtilities.GetWorldPositionFor(position, size, size);
            
            createdUnit.RotationController.LookAtEnemySide();
            createdUnit.BattleMapPlaceable.RelocateTo(_mapHolder.Map[position.x, position.y], _mapHolder.Map);
            
            CreatedUnits.Add(createdUnit);
        }
    }
}