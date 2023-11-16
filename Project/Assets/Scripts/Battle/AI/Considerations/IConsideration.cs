using Battle.UnitCommands.Commands;

namespace Battle.AI.Considerations
{
    public interface IConsideration
    {
        public float ConsiderationResult { get; }
        public bool CalculationComplete { get; }

        public void Consider();

        public ICommand GetCommand();
    }
}