using Battle.StatsSystem;
using Battle.Units.Components.Interfaces;
using Battle.Units.StaticData.Components;
using UnityEngine;
using Utilities.UsefullClasses;

namespace Battle.Units.Components
{
    public class UnitAttack: IStatsInitializer
    {
        private readonly AttackDamageDealerStaticData _staticData;
        private readonly UnitStatsProvider _statsProvider;
        private readonly UnitHealth _health;

        private UnitStat _damageMinStat;
        private UnitStat _damageMaxStat;
        
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
            _damageMinStat = _statsProvider.AddStat(StatType.MinDamage, _staticData.DamageMin);
            _damageMaxStat = _statsProvider.AddStat(StatType.MaxDamage, _staticData.DamageMax);
        }

        public int GetRawDamage()
        {
            var damage = GetMinMaxRawDamageForUnitPack();
            return Random.Range(damage.Min, damage.Max + 1);
        }

        public MinMaxValue GetMinMaxRawDamageForUnitPack()
        {
            var unitsCount = _health.AliveUnitsCount;
            return new MinMaxValue(_damageMinStat.Value * unitsCount, _damageMaxStat.Value * unitsCount);
        }
    }
}