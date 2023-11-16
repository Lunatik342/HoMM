using System;
using Battle.StatsSystem;
using Battle.Units.Components.Interfaces;
using Battle.Units.StaticData.Components;

namespace Battle.Units.Components
{
    public class UnitHealth : IStatsInitializer, IHealthInfoProvider
    {
        private readonly UnitStatsProvider _statsProvider;
        private readonly DamageReceiverStaticData _damageReceiverStaticData;
        private readonly Unit _unit;
        
        private UnitStat _maxHealthStat;

        public int AliveUnitsCount { get; private set; }
        public int CurrenHealth { get; private set; }
        public bool IsAlive => AliveUnitsCount > 0;

        public event Action<int, int> HealthChanged; 
        public event Action<Unit> UnitDied; 

        public UnitHealth(UnitStatsProvider statsProvider, 
            DamageReceiverStaticData damageReceiverStaticData,
            Unit unit)
        {
            _statsProvider = statsProvider;
            _damageReceiverStaticData = damageReceiverStaticData;
            _unit = unit;
        }

        void IStatsInitializer.ConfigureStats()
        {
            _statsProvider.AddStat(StatType.MaxHealth, _damageReceiverStaticData.MaxHealth);
            _statsProvider.AddStat(StatType.Defence, _damageReceiverStaticData.Defence);
            _maxHealthStat = _statsProvider.GetStat(StatType.MaxHealth);
            _maxHealthStat.ValueChanged += AdjustCurrentMaxHealth;
        }

        public void Setup(int count)
        {
            AliveUnitsCount = count;
            CurrenHealth = _statsProvider.GetStatValue(StatType.MaxHealth);
        }

        public void TakeDamage(int damage)
        {
            var damageReceiveData = GetCasualtiesCountForDamage(damage);

            AliveUnitsCount -= damageReceiveData.unitsDied;
            CurrenHealth -= damageReceiveData.healthDamageReceived;
                
            HealthChanged?.Invoke(CurrenHealth, AliveUnitsCount);

            if (AliveUnitsCount == 0 && CurrenHealth == 0)
            {
                UnitDied?.Invoke(_unit);
            }
        }

        public float GetHealthPercentage()
        {
            return (float)CurrenHealth / _maxHealthStat.Value;
        }

        public (int healthDamageReceived, int unitsDied) GetCasualtiesCountForDamage(int damage)
        {
            var aliveUnitsCount = AliveUnitsCount;
            var currentHealth = CurrenHealth;
            
            var maxHealth = _maxHealthStat.Value;
            var healthDamage = damage % maxHealth;
            var unitsDied = damage / maxHealth;

            aliveUnitsCount -= unitsDied;
            currentHealth -= healthDamage;

            if (currentHealth <= 0)
            {
                aliveUnitsCount -= 1;
                currentHealth = maxHealth + currentHealth;
            }

            if (aliveUnitsCount < 1)
            {
                aliveUnitsCount = 0;
                currentHealth = 0;
            }

            return (CurrenHealth - currentHealth, AliveUnitsCount - aliveUnitsCount);
        }

        private void AdjustCurrentMaxHealth(int previousMaxHealth, int currentMaxHealth)
        {
            var healthPercentage = (float)CurrenHealth / previousMaxHealth;
            CurrenHealth = (int)Math.Round(healthPercentage * currentMaxHealth);
            HealthChanged?.Invoke(CurrenHealth, AliveUnitsCount);
        }
    }
}