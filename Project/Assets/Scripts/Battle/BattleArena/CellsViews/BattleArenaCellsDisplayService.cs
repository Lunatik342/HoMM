using RogueSharp;
using UnityEngine;

namespace Battle.BattleArena.CellsViews
{
    public class BattleArenaCellsDisplayService
    {
        private readonly IBattleArenaCellsViewsHolder _cellsViewsHolder;

        public BattleArenaCellsDisplayService(IBattleArenaCellsViewsHolder cellsViewsHolder)
        {
            _cellsViewsHolder = cellsViewsHolder;
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
                        _cellsViewsHolder.CellsViews[i, j].SetUnreachable();
                        continue;
                    }

                    if (cell.IsOccupiedByEntity)
                    {
                        _cellsViewsHolder.CellsViews[i, j].SetObstacle();
                        continue;
                    }
                    
                    _cellsViewsHolder.CellsViews[i, j].SetReachable();
                }
            }
        }
        
        public void DisplayPath(Path path)
        {
            foreach (var cell in path.Steps)
            {
                _cellsViewsHolder.CellsViews[cell.X, cell.Y].SetPath();
            }
        }

        public void SetHover(Vector2Int pos)
        {
            _cellsViewsHolder.CellsViews[pos.x, pos.y].SetHover();
        }

        public void DisplayPrevious(Vector2Int pos)
        {
            _cellsViewsHolder.CellsViews[pos.x, pos.y].RestorePrevState();
        }
    }
}