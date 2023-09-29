using System.Threading.Tasks;
using Battle.BattleField.Cells;
using Infrastructure.AssetManagement;
using Unity.Mathematics;
using UnityEngine;

namespace Battle.BattleField
{
    public class BattleFieldFactory
    {
        private readonly BattleFieldStaticDataService _staticDataService;
        private readonly AssetsLoadingService _assetsLoadingService;
        private readonly BattleFieldCellView _cellViewPrefab;

        private GameObject _battleFieldView;
        public BattleFieldCellView[,] CellViews { get; private set; }

        public BattleFieldFactory(BattleFieldStaticDataService staticDataService, AssetsLoadingService assetsLoadingService, 
            BattleFieldCellView cellViewPrefab)
        {
            _staticDataService = staticDataService;
            _assetsLoadingService = assetsLoadingService;
            _cellViewPrefab = cellViewPrefab;
        }

        public async Task SpawnBattleField(BattleFieldId battleFieldId)
        {
            var battleFieldStaticData = _staticDataService.GetStaticDataForId(battleFieldId);
            
            await SpawnBattleFieldView(battleFieldStaticData);
            SpawnCellViews(battleFieldStaticData);
        }

        private async Task SpawnBattleFieldView(BattleFieldStaticData staticData)
        {
            _battleFieldView = await _assetsLoadingService.Instantiate(staticData.ViewGameObjectReference,
                Vector3.zero - new Vector3(BattleFieldConstants.CellSizeInUnits * 0.5f, 0, BattleFieldConstants.CellSizeInUnits * 0.5f), quaternion.identity, null);
        }

        private void SpawnCellViews(BattleFieldStaticData battleFieldStaticData)
        {
            var size = battleFieldStaticData.Size;
            var cellsParent = new GameObject("CellViews");
            
            CellViews = new BattleFieldCellView[size.x, size.y];

            for (int i = 0; i < size.x; i++)
            {
                for (int j = 0; j < size.y; j++)
                {
                    var heightAboveGround = 0.05f;
                    var createdCell = Object.Instantiate(_cellViewPrefab, new Vector3(i, heightAboveGround, j), Quaternion.identity, cellsParent.transform);
                    CellViews[i, j] = createdCell;
                }
            }
        }
    }
}
