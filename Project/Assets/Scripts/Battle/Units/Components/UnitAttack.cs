using Battle.StatsSystem;
using Battle.Units.Components.Interfaces;
using Battle.Units.StaticData.Components;
using UnityEngine;

namespace Battle.Units.Components
{
    public class UnitAttack: IStatsInitializer
    {
        private readonly AttackDamageDealerStaticData _staticData;
        private readonly UnitStatsProvider _statsProvider;
        private readonly UnitHealth _health;

        private UnitStat _damageMinStat;
        private UnitStat _damageMaxStat;
        private UnitStat _luckStat;
        
        public UnitAttack(AttackDamageDealerStaticData staticData, 
            UnitStatsProvider statsProvider, 
            UnitHealth health)
        {
            _staticData = staticData;
            _statsProvider = statsProvider;
            _health = health;
        }
        
        void IStatsInitializer.ConfigureStats()
        {
            _statsProvider.AddStat(StatType.Attack, _staticData.AttackStat);
            _luckStat = _statsProvider.AddStat(StatType.Luck, _staticData.Luck);
            _damageMinStat = _statsProvider.AddStat(StatType.MinDamage, _staticData.DamageMin);
            _damageMaxStat = _statsProvider.AddStat(StatType.MaxDamage, _staticData.DamageMax);
        }

        public int GetRawDamage()
        {
            var unitsCount = _health.AliveUnitsCount;
            var damageFromOneUnit = Random.Range(_damageMinStat.Value, _damageMaxStat.Value + 1);

            var luckValue = Random.Range(0, 10);

            if (luckValue < _luckStat.Value)
            {
                int critMultiplier = 2;
                damageFromOneUnit *= critMultiplier;
            }

            return damageFromOneUnit * unitsCount;
        }
    }
}