using Battle.BattleArena.StaticData;
using UnityEngine;
using Zenject;

namespace Battle.BattleArena.CellsViews
{
    public class BattleArenaCellsViewsFactory: IFactory<BattleArenaId, BattleArenaCellView[,]>
    {
        private const float CellsViewHeightAboveGround = 0.05f;
        
        private readonly BattleArenaCellView.Factory _cellViewFactory;
        private readonly BattleArenaStaticDataProvider _staticDataProvider;

        public BattleArenaCellsViewsFactory(
            BattleArenaCellView.Factory cellViewFactory,
            BattleArenaStaticDataProvider staticDataProvider)
        {
            _cellViewFactory = cellViewFactory;
            _staticDataProvider = staticDataProvider;
        }
        
        public BattleArenaCellView[,] Create(BattleArenaId battleArenaId)
        {
            var arenaSize = _staticDataProvider.ForBattleArena(battleArenaId).Size;
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

    public class CellsViewsArrayFactory : PlaceholderFactory<BattleArenaId, BattleArenaCellView[,]>
    {
        
    }
}