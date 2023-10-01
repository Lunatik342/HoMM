using System.Threading.Tasks;
using Battle.BattleArena.Cells;
using Battle.BattleArena.StaticData;
using Infrastructure.AssetManagement;
using Unity.Mathematics;
using UnityEngine;

namespace Battle.BattleArena
{
    public class BattleFieldViewSpawner
    {
        private readonly BattleArenaStaticDataProvider _staticDataProvider;
        private readonly AssetsLoadingService _assetsLoadingService;
        private readonly BattleStartParameters _battleStartParameters;

        private Transform _battleFieldView;

        public BattleFieldViewSpawner(
            BattleArenaStaticDataProvider staticDataProvider, 
            AssetsLoadingService assetsLoadingService, 
            BattleStartParameters battleStartParameters)
        {
            _staticDataProvider = staticDataProvider;
            _assetsLoadingService = assetsLoadingService;
            _battleStartParameters = battleStartParameters;
        }

        public async Task SpawnBattleField()
        {
            var battleFieldStaticData = _staticDataProvider.ForBattleArena(_battleStartParameters.BattleArenaId);
            await SpawnBattleFieldView(battleFieldStaticData);
        }

        private async Task SpawnBattleFieldView(BattleArenaStaticData arenaStaticData)
        {
            //Taking [0,0] cell's top left corner as starting position for the view
            var gridPosition = new Vector2(-0.5f, -0.5f);
            
            _battleFieldView = await _assetsLoadingService.InstantiateAsync<Transform>(arenaStaticData.ViewGameObjectReference,
                gridPosition.ToBattleArenaWorldPosition(), quaternion.identity, null);
        }
    }
}
