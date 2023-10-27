using System;
using Battle.BattleArena.Pathfinding.StaticData;
using Battle.Units.StatsSystem;

namespace Battle.Units
{
    public class UnitHealth : IStatsInitializer
    {
        private readonly UnitStatsProvider _statsProvider;
        private readonly DamageReceiverStaticData _damageReceiverStaticData;
        private UnitStat _maxHealthStat;

        public int AliveUnitsCount { get; private set; }
        public int CurrenHealth { get; private set; }
        public bool IsAlive => AliveUnitsCount > 0;

        public event Action<int, int> HealthChanged; 
        public event Action UnitDied; 

        public UnitHealth(UnitStatsProvider statsProvider, 
            DamageReceiverStaticData damageReceiverStaticData)
        {
            _statsProvider = statsProvider;
            _damageReceiverStaticData = damageReceiverStaticData;
        }

        void IStatsInitializer.ConfigureStats()
        {
            _statsProvider.AddStat(StatType.MaxHealth, _damageReceiverStaticData.MaxHealth);
            _statsProvider.AddStat(StatType.Defence, _damageReceiverStaticData.Defence);
            _maxHealthStat = _statsProvider.GetStat(StatType.MaxHealth);
            _maxHealthStat.ValueChanged += AdjustCurrentMaxHealth;
        }

        public void SetUnitsCount(int count)
        {
            AliveUnitsCount = count;
            CurrenHealth = _statsProvider.GetStatValue(StatType.MaxHealth);
        }

        public void TakeDamage(int damage)
        {
            var maxHealth = _maxHealthStat.Value;
            
            var healthDamage = damage % maxHealth;
            var unitsDied = damage / maxHealth;

            AliveUnitsCount -= unitsDied;
            CurrenHealth -= healthDamage;

            if (CurrenHealth <= 0)
            {
                AliveUnitsCount -= 1;
                CurrenHealth = maxHealth + CurrenHealth;
            }

            if (AliveUnitsCount < 1)
            {
                AliveUnitsCount = 0;
                CurrenHealth = 0;
            }
            
            HealthChanged?.Invoke(CurrenHealth, AliveUnitsCount);

            if (AliveUnitsCount == 0 && CurrenHealth == 0)
            {
                UnitDied?.Invoke();
            }
        }

        public float GetHealthPercentage()
        {
            return (float)CurrenHealth / _maxHealthStat.Value;
        }

        private void AdjustCurrentMaxHealth(int previousMaxHealth, int currentMaxHealth)
        {
            var healthPercentage = (float)CurrenHealth / previousMaxHealth;
            CurrenHealth = (int)Math.Round(healthPercentage * currentMaxHealth);
            HealthChanged?.Invoke(CurrenHealth, AliveUnitsCount);
        }
    }
}