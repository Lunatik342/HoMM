using RogueSharp;
using UnityEngine;

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

                    if (cell.IsOccupiedByEntity)
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

        public void SetHover(Vector2Int pos)
        {
            _cellsViews[pos.x, pos.y].SetHover();
        }

        public void DisplayPrevious(Vector2Int pos)
        {
            _cellsViews[pos.x, pos.y].RestorePrevState();
        }
    }
}