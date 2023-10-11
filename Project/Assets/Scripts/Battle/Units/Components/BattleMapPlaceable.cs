using System.Collections.Generic;
using Battle.BattleArena.Pathfinding.StaticData;
using Battle.Units.Movement;
using RogueSharp;

namespace Battle.BattleArena.Pathfinding
{
    public class BattleMapPlaceable: IDeathEventReceiver
    {
        private readonly UnitGridPlaceableStaticData _staticData;

        public int Size => _staticData.Size;
        public List<Cell> OccupiedCells { get; private set; } = new();

        public BattleMapPlaceable(UnitGridPlaceableStaticData staticData)
        {
            _staticData = staticData;
        }

        public void RelocateTo(List<Cell> targetCells)
        {
            RemoveFromCurrentPosition();
            
            foreach (var cell in targetCells) 
            {
                cell.PlaceEntity(this);
            }

            OccupiedCells = targetCells;
        }
        
        public void RelocateTo(Cell topLeftCorner, Map map)
        {
            List<Cell> occupiedCells = new List<Cell>(Size * Size);
            
            for (int i = topLeftCorner.X; i < topLeftCorner.X + Size; i++)
            {
                for (int j = topLeftCorner.Y; j < topLeftCorner.Y + Size; j++)
                {
                    occupiedCells.Add(map[i, j]);
                }
            }

            RelocateTo(occupiedCells);
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