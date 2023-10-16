using System.Collections.Generic;
using Battle.BattleArena.Pathfinding;

namespace Battle.Units
{
    public interface IUnitsHolder
    {
        public List<Unit> AllUnits { get; }
    }
}