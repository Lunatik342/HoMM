using Battle.Units;
using Utilities.UsefullClasses;

namespace Battle.DamageCalculation
{
    public class DamagePredictionService
    {
        private readonly DamageCalculator _damageCalculator;

        public DamagePredictionService(DamageCalculator damageCalculator)
        {
            _damageCalculator = damageCalculator;
        }
        
        public MinMaxValue PredictDamage(Unit attackingUnit, Unit attackedUnit)
        {
            var rawDamage = attackingUnit.Attack.GetMinMaxRawDamageForUnitPack();

            var finalDamage = new MinMaxValue(
                _damageCalculator.MultiplyRawDamageByModifiers(attackedUnit, attackedUnit, rawDamage.Min),
                _damageCalculator.MultiplyRawDamageByModifiers(attackedUnit, attackedUnit, rawDamage.Max));
            
            var (_, unitsDiedMin) = attackedUnit.Health.GetCasualtiesCountForDamage(finalDamage.Min);
            var (_, unitsDiedMax) = attackedUnit.Health.GetCasualtiesCountForDamage(finalDamage.Max);
            return new MinMaxValue(unitsDiedMin, unitsDiedMax);
        }
    }
}
