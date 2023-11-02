using Battle.BattleFlow.Phases;
using Infrastructure.SimpleStateMachine;
using Zenject;

namespace Battle
{
    public class BattleBootstrapper: IInitializable
    {
        private readonly BattlePhasesStateMachine _battlePhasesStateMachine;
        private readonly StatesFactory _statesFactory;
        private readonly BattleStartParameters _battleStartParameters;

        public BattleBootstrapper(BattlePhasesStateMachine battlePhasesStateMachine, 
            StatesFactory statesFactory,
            BattleStartParameters battleStartParameters)
        {
            _battlePhasesStateMachine = battlePhasesStateMachine;
            _statesFactory = statesFactory;
            _battleStartParameters = battleStartParameters;
        }

        public void Initialize()
        {
            _battlePhasesStateMachine.RegisterState(_statesFactory.Create<BattleStartPhase>());
            _battlePhasesStateMachine.RegisterState(_statesFactory.Create<BattleProgressPhase>());
            _battlePhasesStateMachine.RegisterState(_statesFactory.Create<BattleEndPhase>());
            
            _battlePhasesStateMachine.Enter<BattleStartPhase, BattleStartParameters>(_battleStartParameters);
        }
    }
}