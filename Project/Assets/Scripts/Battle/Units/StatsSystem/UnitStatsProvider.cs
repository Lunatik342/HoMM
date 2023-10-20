using System.Collections.Generic;

namespace Battle.Units.StatsSystem
{
    public class UnitStatsProvider
    {
        private Dictionary<StatType, UnitStat> _stats = new();

        public void AddStat(StatType statType, int baseValue)
        {
            _stats.Add(statType, new UnitStat(baseValue));
        }

        public bool TryGetStat(StatType statType, out UnitStat unitStat)
        {
            return _stats.TryGetValue(statType, out unitStat);
        }
        
        public UnitStat GetStat(StatType statType)
        {
            return _stats[statType];
        }
        
        public int GetStatValue(StatType statType)
        {
            return _stats[statType].Value;
        }
    }

    public enum StatType
    {
        TravelDistance = 1,
    }
}