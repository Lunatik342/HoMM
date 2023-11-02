using Infrastructure.SimpleStateMachine;
using UnityEngine;

namespace Battle.BattleFlow.Phases
{
    public class BattleEndPhase: IState
    {
        public void Enter()
        {
            Debug.LogError("Game over");
        }

        public void Exit()
        {
            
        }
    }
}