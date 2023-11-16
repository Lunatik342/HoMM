using Battle.StatsSystem;
using Battle.Units;

namespace Battle
{
    public class DamageCalculator
    {
        public int CalculateDamage(Unit attackingUnit, Unit attackedUnit)
        {
            var rawDamage = attackingUnit.Attack.GetRawDamage();
            return MultiplyRawDamage(attackingUnit, attackedUnit, rawDamage);
        }

        public int MultiplyRawDamage(Unit attackingUnit, Unit attackedUnit, int rawDamage)
        {
            var attack = attackingUnit.StatsProvider.GetStatValue(StatType.Attack);
            var defence = attackedUnit.StatsProvider.GetStatValue(StatType.Defence);
            
            var damage = attack > defence
                ? rawDamage * (1 + 0.05f * (attack - defence))
                : rawDamage / (1 + 0.05f * (defence - attack));

            return (int) damage;
        }
    }
}
