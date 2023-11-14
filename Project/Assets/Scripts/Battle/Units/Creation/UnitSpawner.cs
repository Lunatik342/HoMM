using Battle.Arena.Map;
using Battle.Arena.Misc;
using Battle.Units.StaticData;
using Cysharp.Threading.Tasks;
using Infrastructure.AssetManagement;
using UnityEngine;

namespace Battle.Units.Creation
{
    public class UnitSpawner
    {
        private readonly Unit.Factory _unitsFactory;
        private readonly UnitsStaticDataProvider _unitsStaticDataProvider;
        private readonly AssetsLoadingService _assetsLoadingService;
        private readonly IMapHolder _mapHolder;

        public UnitSpawner(Unit.Factory unitsFactory, 
            UnitsStaticDataProvider unitsStaticDataProvider, 
            AssetsLoadingService assetsLoadingService,
            IMapHolder mapHolder)
        {
            _unitsFactory = unitsFactory;
            _unitsStaticDataProvider = unitsStaticDataProvider;
            _assetsLoadingService = assetsLoadingService;
            _mapHolder = mapHolder;
        }
        
        public async UniTask<Unit> Create(UnitCreationParameter unitCreationParameter, Team team)
        {
            var unitStaticData = _unitsStaticDataProvider.ForUnit(unitCreationParameter.UnitId);
            var gridPosition = unitCreationParameter.Position;
            
            var gameObject = await _assetsLoadingService.InstantiateAsync(unitStaticData.GameObjectAssetReference, 
                gridPosition.ToBattleArenaWorldPosition(), Quaternion.identity, null);
            
            var createdUnit = _unitsFactory.Create(gameObject.gameObject, team, unitStaticData);

            createdUnit.Initialize(_mapHolder.Map.GetCell(gridPosition), unitCreationParameter.Count);

            return createdUnit;
        }
    }
}