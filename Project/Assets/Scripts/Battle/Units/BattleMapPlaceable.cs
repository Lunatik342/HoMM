using System;
using Battle.Units.Movement;
using Infrastructure.AssetManagement;
using RogueSharp;

namespace Battle.BattleArena.Pathfinding
{
    public class BattleMapPlaceable: IDeathEventReceiver
    {
        public Cell[] OccupiedCells { get; private set; } = Array.Empty<Cell>();

        public void RelocateTo(Cell[] targetCells)
        {
            RemoveFromCurrentPosition();
            
            foreach (var cell in targetCells) 
            {
                cell.PlaceEntity(this);
            }

            OccupiedCells = targetCells;
        }

        public void OnDeath()
        {
            RemoveFromCurrentPosition();
        }

        private void RemoveFromCurrentPosition()
        {
            foreach (var occupiedCell in OccupiedCells)
            {
                occupiedCell.RemoveEntity(this);
            }
        }
    }
}