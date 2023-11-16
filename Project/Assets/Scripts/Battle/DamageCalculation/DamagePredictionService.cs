using Battle.Units;
using Battle.Units.Components;

namespace Battle.AI
{
    public class DamagePredictionService
    {
        public MinMaxValue PredictDamage(Unit attackingUnit, Unit attackedUnit)
        {
            var rawDamage = attackingUnit.Attack.GetMinMaxRawDamageForUnitPack();

            var (_, unitsDiedMin) = attackedUnit.Health.GetCasualtiesCountForDamage(rawDamage.Min);
            var (_, unitsDiedMax) = attackedUnit.Health.GetCasualtiesCountForDamage(rawDamage.Max);
            return new MinMaxValue(unitsDiedMin, unitsDiedMax);
        }
    }
}
