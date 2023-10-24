using System;

namespace Battle.BattleFlow.StateMachine.MouseOverCells
{
    public class EmptyCellHoverHandler: ICellHoverHandler
    {
        public void Start(Action repaintAction)
        {
            repaintAction();
        }
        
        public void Clear()
        {
            
        }
    }
}