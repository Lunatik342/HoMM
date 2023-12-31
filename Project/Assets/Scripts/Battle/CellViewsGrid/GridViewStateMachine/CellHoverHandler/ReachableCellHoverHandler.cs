using System;
using System.Threading.Tasks;
using Algorithms.RogueSharp;
using Battle.CellViewsGrid.CellsViews;
using Battle.CellViewsGrid.PathDisplay;
using Battle.Input;
using Battle.UnitCommands.Commands;
using Battle.Units;

namespace Battle.CellViewsGrid.GridViewStateMachine.CellHoverHandler
{
    public class ReachableCellHoverHandler: ICellHoverHandler
    {
        private readonly BattleArenaCellsDisplayService _cellsDisplayService;
        private readonly PathDisplayService _pathDisplayService;
        private readonly BattleCellsInputService _cellsInputService;
        
        private TaskCompletionSource<ICommand> _commandCompletionSource;
        private Unit _controlledUnit;

        public ReachableCellHoverHandler(BattleArenaCellsDisplayService cellsDisplayService,
            PathDisplayService pathDisplayService,
            BattleCellsInputService cellsInputService)
        {
            _cellsDisplayService = cellsDisplayService;
            _pathDisplayService = pathDisplayService;
            _cellsInputService = cellsInputService;
        }

        public void Start(TaskCompletionSource<ICommand> commandCompletionSource, Action repaintCellsAction, Unit controlledUnit, Cell mouseoverCell)
        {
            _controlledUnit = controlledUnit;
            _commandCompletionSource = commandCompletionSource;
            
            repaintCellsAction();
            _cellsDisplayService.DisplayMoveTargetCell(mouseoverCell);
            controlledUnit.MovementController.DisplayPathToCell(_pathDisplayService, mouseoverCell.GridPosition);
            
            _cellsInputService.CellLeftClicked += OnCellClicked;
        }

        private void OnCellClicked(Cell cellClicked)
        {
            _commandCompletionSource.SetResult(new UnitMoveCommand(_controlledUnit, cellClicked.GridPosition));
        }

        public void Clear()
        {
            _pathDisplayService.StopDisplaying();
            _cellsInputService.CellLeftClicked -= OnCellClicked;
        }
    }
}