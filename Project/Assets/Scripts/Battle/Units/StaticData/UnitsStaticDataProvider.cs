using System.Collections.Generic;
using System.Linq;

namespace Battle.BattleArena.Pathfinding.StaticData
{
    public class UnitsStaticDataProvider
    {
        private Dictionary<UnitId, UnitStaticData> _unitsStaticData;

        public UnitsStaticDataProvider(List<UnitStaticData> allUnits)
        {
            _unitsStaticData = allUnits.ToDictionary(data => data.UnitId);
        }

        public UnitStaticData ForUnit(UnitId unitId)
        {
            return _unitsStaticData[unitId];
        }
    }
}