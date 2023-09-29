using System.Collections.Generic;
using System.Linq;

namespace Battle.BattleField
{
    public class BattleFieldStaticDataService
    {
        private readonly Dictionary<BattleFieldId, BattleFieldStaticData> _staticData;

        public BattleFieldStaticDataService(List<BattleFieldStaticData> staticData)
        {
            _staticData = staticData.ToDictionary(s => s.Id);
        }

        public BattleFieldStaticData GetStaticDataForId(BattleFieldId id)
        {
            return _staticData[id];
        }
    }
}