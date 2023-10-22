using Battle.BattleArena.Pathfinding.StaticData;
using Battle.Units.Movement;
using RogueSharp;

namespace Battle.BattleArena.Pathfinding
{
    public class BattleMapPlaceable : IDeathEventReceiver
    {
        private readonly UnitGridPlaceableStaticData _staticData;
        private readonly Unit _unit;

        public int Size => _staticData.Size;
        public Cell OccupiedCell { get; private set; }

        public BattleMapPlaceable(UnitGridPlaceableStaticData staticData, Unit unit)
        {
            _staticData = staticData;
            _unit = unit;
        }

        public void RelocateTo(Cell targetCell)
        {
            RemoveFromCurrentPosition();
            targetCell.PlaceEntity(_unit);
            OccupiedCell = targetCell;
        }

        public void OnDeath()
        {
            RemoveFromCurrentPosition();
        }

        private void RemoveFromCurrentPosition()
        {
            OccupiedCell?.RemoveEntity(_unit);
        }
    }
}