using System.Collections.Generic;

namespace Battle.Units.StatsSystem
{
    public class UnitStatsProvider
    {
        private Dictionary<StatType, UnitStat> _stats = new();

        public UnitStat AddStat(StatType statType, int baseValue)
        {
            var stat = new UnitStat(baseValue);
            _stats.Add(statType, stat);
            return stat;
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
        Initiative = 2,
        MaxHealth = 3,
        Defence = 4,
        Attack = 5,
        MinDamage = 6,
        MaxDamage = 7,
        Luck = 8
    }
}