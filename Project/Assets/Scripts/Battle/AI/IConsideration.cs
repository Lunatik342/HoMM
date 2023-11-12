using Battle.UnitCommands.Commands;

namespace Battle.AI
{
    public interface IConsideration
    {
        public float ConsiderationResult { get; }
        public bool CalculationComplete { get; }

        public void Consider();

        public ICommand GetCommand();
    }
}