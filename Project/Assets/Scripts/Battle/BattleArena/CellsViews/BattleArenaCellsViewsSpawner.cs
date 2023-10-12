using Battle.BattleArena.StaticData;
using Zenject;

namespace Battle.BattleArena.CellsViews
{
    public class BattleArenaCellsViewsSpawner: IBattleArenaCellsViewsHolder
    {
        private readonly IFactory<BattleArenaId, BattleArenaCellView[,]> _cellsViewsFactory;

        public BattleArenaCellView[,] CellsViews { get; private set; }

        public BattleArenaCellsViewsSpawner(CellsViewsArrayFactory cellsViewsFactory)
        {
            _cellsViewsFactory = cellsViewsFactory;
        }

        public void Spawn(BattleArenaId battleArenaId)
        {
            CellsViews = _cellsViewsFactory.Create(battleArenaId);
        }
    }

    public interface IBattleArenaCellsViewsHolder
    {
        public BattleArenaCellView[,] CellsViews { get; }
    }
}