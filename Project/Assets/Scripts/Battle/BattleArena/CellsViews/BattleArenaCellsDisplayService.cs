using System.Linq;
using RogueSharp;
using RogueSharp.Algorithms;
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
            var kek = new KekPath<Cell>(1.41f);

            var cells = kek.FindPath(map[0, 0], map, null, 8);

            for (int i = 0; i < cells.Length; i++)
            {
                var cell = map.CellFor(i);
                
                if (!cells[i])
                {
                    _cellsViewsHolder.CellsViews[cell.X, cell.Y].SetReachable();
                    continue;
                }
                
                _cellsViewsHolder.CellsViews[cell.X, cell.Y].SetPath();
            }
            
            Debug.LogError(cells.Count(t => t));
            return;
            
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
            return;
            
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