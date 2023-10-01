using Battle.BattleArena.StaticData;
using UnityEngine;
using Zenject;

namespace Battle.BattleArena.Cells
{
    public class BattleArenaCellsViewsFactory: IFactory<BattleArenaCellView[,]>
    {
        private const float CellsViewHeightAboveGround = 0.05f;
        
        private readonly BattleArenaCellView.Factory _cellViewFactory;
        private readonly BattleStartParameters _battleStartParameters;
        private readonly BattleArenaStaticDataProvider _staticDataProvider;

        public BattleArenaCellsViewsFactory(
            BattleArenaCellView.Factory cellViewFactory, 
            BattleStartParameters battleStartParameters, 
            BattleArenaStaticDataProvider staticDataProvider)
        {
            _cellViewFactory = cellViewFactory;
            _battleStartParameters = battleStartParameters;
            _staticDataProvider = staticDataProvider;
        }
        
        public BattleArenaCellView[,] Create()
        {
            var arenaSize = _staticDataProvider.ForBattleArena(_battleStartParameters.BattleArenaId).Size;
            var cellViews = new BattleArenaCellView[arenaSize.x, arenaSize.y];

            for (int i = 0; i < arenaSize.x; i++)
            {
                for (int j = 0; j < arenaSize.y; j++)
                {
                    var createdCell = _cellViewFactory.Create();
                    cellViews[i, j] = createdCell;
                    
                    var gridPosition = new Vector2Int(i, j);
                    createdCell.transform.position = gridPosition.ToBattleArenaWorldPosition() + Vector3.up * CellsViewHeightAboveGround;
                }
            }

            return cellViews;
        }
    }
}