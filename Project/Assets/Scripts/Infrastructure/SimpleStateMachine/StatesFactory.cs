using Zenject;

namespace Infrastructure.SimpleStateMachine
{
    public class StatesFactory
    {
        private IInstantiator _instantiator;

        public StatesFactory(IInstantiator instantiator)
        {
            _instantiator = instantiator;
        }

        public TState Create<TState>() where TState : IExitableState
        {
            return _instantiator.Instantiate<TState>();
        }
    }
}