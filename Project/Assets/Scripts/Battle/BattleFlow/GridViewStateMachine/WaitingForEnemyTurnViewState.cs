using System.Collections.Generic;
using Battle.BattleArena.CellsViews;
using Infrastructure.SimpleStateMachine;
using RogueSharp;

namespace Battle.BattleFlow.StateMachine
{
    public class WaitingForEnemyTurnViewState: DisplayingCellsViewStateBase, IState
    {
        private readonly BattleCellsInputService _cellsInputService;
        private readonly BattleArenaCellsDisplayService _cellsDisplayService;

        public WaitingForEnemyTurnViewState(BattleCellsInputService cellsInputService, 
            BattleArenaCellsDisplayService cellsDisplayService) : base(cellsDisplayService)
        {
            _cellsInputService = cellsInputService;
            _cellsDisplayService = cellsDisplayService;
        }
        
        public void Enter()
        {
            _cellsInputService.MouseoverCellChanged += MouseoverCellChanged;
            MouseoverCellChanged(_cellsInputService.MouseOverCell);
        }

        private void MouseoverCellChanged(Cell cell)
        {
            _cellsDisplayService.DisplayAllCellsDefault();
            DisplayReachableCells(cell, new List<Cell>());
        }

        public void Exit()
        {
            _cellsInputService.MouseoverCellChanged -= MouseoverCellChanged;
        }
    }

}