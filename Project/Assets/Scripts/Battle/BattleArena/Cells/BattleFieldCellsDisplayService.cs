using RogueSharp;

namespace Battle.BattleArena.Cells
{
    public class BattleFieldCellsDisplayService
    {
        private readonly BattleFieldFactory _battleFieldFactory;

        public BattleFieldCellsDisplayService(BattleFieldFactory battleFieldFactory)
        {
            _battleFieldFactory = battleFieldFactory;
        }

        public void DisplayBattleField(Map map)
        {
            var cellsViews = _battleFieldFactory.CellViews;
            
            for (int i = 0; i < map.Width; i++)
            {
                for (int j = 0; j < map.Height; j++)
                {
                    var cell = map[i, j];

                    if (cell.IsWalkable)
                    {
                        cellsViews[i, j].SetReachable();
                    }
                    else
                    {
                        cellsViews[i, j].SetUnreachable();
                    }
                }
            }
        }
        
        public void DisplayPath(Path path)
        {
            var cellsViews = _battleFieldFactory.CellViews;

            foreach (var cell in path.Steps)
            {
                cellsViews[cell.X, cell.Y].SetPath();
            }
        }
    }
}