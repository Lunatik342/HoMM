using Battle.BattleArena.Pathfinding.StaticData;
using Battle.Units.Movement;
using RogueSharp;

namespace Battle.BattleArena.Pathfinding
{
    public class BattleMapPlaceable : IDeathEventReceiver, IUnitPositionProvider
    {
        private readonly UnitGridPlaceableStaticData _staticData;
        private readonly Unit _unit;

        public Cell OccupiedCell { get; private set; }

        public BattleMapPlaceable(UnitGridPlaceableStaticData staticData, Unit unit)
        {
            _staticData = staticData;
            _unit = unit;
        }

        public void RelocateTo(Cell targetCell)
        {
            RemoveFromCurrentPosition();
            targetCell.PlaceUnit(_unit);
            OccupiedCell = targetCell;
        }

        void IDeathEventReceiver.OnDeath()
        {
            RemoveFromCurrentPosition();
        }

        private void RemoveFromCurrentPosition()
        {
            OccupiedCell?.RemoveUnit(_unit);
        }
    }
}