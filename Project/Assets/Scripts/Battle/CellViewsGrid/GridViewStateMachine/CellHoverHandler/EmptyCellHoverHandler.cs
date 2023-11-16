using System;

namespace Battle.CellViewsGrid.GridViewStateMachine.CellHoverHandler
{
    public class EmptyCellHoverHandler: ICellHoverHandler
    {
        public void OnHover(Action repaintAction)
        {
            repaintAction();
        }
        
        public void Clear()
        {
            
        }
    }
}