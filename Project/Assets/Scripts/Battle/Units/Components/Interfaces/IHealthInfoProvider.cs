using System;

namespace Battle.Units
{
    public interface IHealthInfoProvider
    {
        int AliveUnitsCount { get; }
        int CurrenHealth { get; }
        bool IsAlive { get; }
        event Action<int, int> HealthChanged;
        event Action UnitDied;
        float GetHealthPercentage();
    }
}