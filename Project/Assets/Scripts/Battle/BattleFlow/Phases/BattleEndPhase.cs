using Infrastructure.SimpleStateMachine;
using UnityEngine;

namespace Battle.BattleFlow.Phases
{
    public class BattleEndPhase: IPaylodedState<Team>
    {

        public void Enter(Team payload)
        {
            Debug.LogError($"Game over: {payload} won");
        }

        public void Exit()
        {
            
        }
    }
}