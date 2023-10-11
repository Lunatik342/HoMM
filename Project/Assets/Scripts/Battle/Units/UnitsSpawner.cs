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
        private readonly Unit.Factory _factory;
        private readonly UnitsStaticDataProvider _unitsStaticDataProvider;
        private readonly AssetsLoadingService _assetsLoadingService;
        private readonly Map _map;

        public List<Unit> CreatedUnits = new();

        public UnitsSpawner(Unit.Factory factory, 
            UnitsStaticDataProvider unitsStaticDataProvider, 
            AssetsLoadingService assetsLoadingService,
            Map map)
        {
            _factory = factory;
            _unitsStaticDataProvider = unitsStaticDataProvider;
            _assetsLoadingService = assetsLoadingService;
            _map = map;
        }
        
        public async UniTask Create(UnitId unitId, Vector2Int position, Team team)
        {
            var unitStaticData = _unitsStaticDataProvider.ForUnit(unitId);
            
            var gameObject = await _assetsLoadingService.InstantiateAsync(unitStaticData.GameObjectAssetReference, 
                position.ToBattleArenaWorldPosition(), Quaternion.identity, null);
            
            var createdUnit = _factory.Create(gameObject.gameObject, team, unitStaticData);
            
            var size = createdUnit.BattleMapPlaceable.Size;
            createdUnit.GameObject.transform.position = TwoDimensionalArrayUtilities.GetWorldPositionFor(position, size, size);
            
            createdUnit.RotationController.LookAtEnemySide();
            createdUnit.BattleMapPlaceable.RelocateTo(_map[position.x, position.y], _map);
            
            CreatedUnits.Add(createdUnit);
        }
    }
}