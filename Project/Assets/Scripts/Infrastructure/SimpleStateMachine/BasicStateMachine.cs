using System;
using System.Collections.Generic;

namespace Infrastructure.SimpleStateMachine
{
    public class BasicStateMachine : IStateMachine
    {
        private readonly Dictionary<Type, IExitableState> _registeredStates;
        private IExitableState _currentState;

        public BasicStateMachine()
        {
            _registeredStates = new Dictionary<Type, IExitableState>();
        }

        public void RegisterState<TState>(TState state) where TState : IExitableState
        {
            _registeredStates.Add(typeof(TState), state);
        }

        public void Enter<TState>() where TState : class, IState
        {
            TState newState = ChangeState<TState>();
            newState.Enter();
        }

        public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPaylodedState<TPayload>
        {
            TState newState = ChangeState<TState>();
            newState.Enter(payload);
        }

        private TState ChangeState<TState>() where TState : class, IExitableState
        {
            _currentState?.Exit();
      
            TState state = GetState<TState>();
            _currentState = state;
      
            return state;
        }

        private TState GetState<TState>() where TState : class, IExitableState
        {
            return _registeredStates[typeof(TState)] as TState;
        }
    }
}