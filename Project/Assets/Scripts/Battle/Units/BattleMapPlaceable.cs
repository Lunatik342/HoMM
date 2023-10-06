using System.Collections.Generic;
using RogueSharp;

namespace Battle.BattleArena.Pathfinding
{
    public class BattleMapPlaceable
    {
        public int Size { get; private set; }
        public List<Cell> OccupiedCells { get; private set; } = new List<Cell>();

        public BattleMapPlaceable(int size)
        {
            Size = size;
        }

        public void Relocate(List<Cell> targetCells)
        {
            RemoveFromCurrentPosition();
            
            foreach (var cell in targetCells) 
            {
                cell.PlaceEntity(this);
            }

            OccupiedCells = targetCells;
        }

        public void RemoveFromCurrentPosition()
        {
            foreach (var occupiedCell in OccupiedCells)
            {
                occupiedCell.RemoveEntity(this);
            }
        }
    }
}