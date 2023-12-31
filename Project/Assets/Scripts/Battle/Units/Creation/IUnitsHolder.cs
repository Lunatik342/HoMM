using System.Collections.Generic;

namespace Battle.Units.Creation
{
    public interface IUnitsHolder
    {
        public IEnumerable<Unit> GetAllUnits();
        public IEnumerable<Unit> GetAllAliveUnits();

        public List<Unit> GetAllUnitsOfTeam(Team team);
        public List<Unit> GetAllAliveUnitsOfTeam(Team team);
    }
}