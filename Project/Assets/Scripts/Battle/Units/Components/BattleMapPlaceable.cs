using System.Collections.Generic;
using Battle.BattleArena.Pathfinding.StaticData;
using Battle.Units.Movement;
using RogueSharp;

namespace Battle.BattleArena.Pathfinding
{
    public class BattleMapPlaceable: IDeathEventReceiver
    {
        private readonly UnitGridPlaceableStaticData _staticData;
        private readonly Unit _unit;

        public int Size => _staticData.Size;
        public List<Cell> OccupiedCells { get; private set; } = new();
        
        //Костыль на время пока у меня нет юнитов с размером больше единицы
        public Cell OccupiedCell => OccupiedCells[0];

        public BattleMapPlaceable(UnitGridPlaceableStaticData staticData, Unit unit)
        {
            _staticData = staticData;
            _unit = unit;
        }

        public void RelocateTo(List<Cell> targetCells)
        {
            RemoveFromCurrentPosition();
            
            foreach (var cell in targetCells) 
            {
                cell.PlaceEntity(_unit);
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
                occupiedCell.RemoveEntity(_unit);
            }
        }
    }
}