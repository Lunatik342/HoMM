namespace Infrastructure.SimpleStateMachine
{
    public interface IState : IExitableState
    {
        void Enter();
    }
}