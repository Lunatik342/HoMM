using RogueSharp;

namespace Battle.BattleArena.Cells
{
    public class BattleArenaCellsDisplayService
    {
        private readonly BattleArenaCellView[,] _cellsViews;

        public BattleArenaCellsDisplayService(BattleArenaCellView[,] cellsViews)
        {
            _cellsViews = cellsViews;
        }

        public void DisplayBattleField(Map map)
        {
            for (int i = 0; i < map.Width; i++)
            {
                for (int j = 0; j < map.Height; j++)
                {
                    var cell = map[i, j];

                    if (cell.IsWalkable)
                    {
                        _cellsViews[i, j].SetReachable();
                    }
                    else
                    {
                        _cellsViews[i, j].SetUnreachable();
                    }
                }
            }
        }
        
        public void DisplayPath(Path path)
        {
            foreach (var cell in path.Steps)
            {
                _cellsViews[cell.X, cell.Y].SetPath();
            }
        }
    }
}