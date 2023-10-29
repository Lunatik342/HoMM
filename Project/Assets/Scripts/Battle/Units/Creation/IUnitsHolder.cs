using System.Collections.Generic;
using Battle.BattleArena.Pathfinding;

namespace Battle.Units
{
    public interface IUnitsHolder
    {
        public IEnumerable<Unit> GetAllUnits();

        public List<Unit> GetAllUnitsOfTeam(Team team);
    }
}