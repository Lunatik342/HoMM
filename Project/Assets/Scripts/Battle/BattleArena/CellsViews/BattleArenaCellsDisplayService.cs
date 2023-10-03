using RogueSharp;

namespace Battle.BattleArena.CellsViews
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

                    if (!cell.IsFunctioning)
                    {
                        _cellsViews[i, j].SetUnreachable();
                        continue;
                    }

                    if (cell.IsOccupied)
                    {
                        _cellsViews[i, j].SetObstacle();
                        continue;
                    }
                    
                    _cellsViews[i, j].SetReachable();
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