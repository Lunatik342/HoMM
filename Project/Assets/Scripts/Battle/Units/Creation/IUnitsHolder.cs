using System.Collections.Generic;

namespace Battle.Units.Creation
{
    public interface IUnitsHolder
    {
        public IEnumerable<Unit> GetAllUnits();

        public List<Unit> GetAllUnitsOfTeam(Team team);
    }
}