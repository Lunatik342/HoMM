using System;
using System.Threading.Tasks;
using Battle.BattleArena.CellsViews;
using Battle.BattleArena.PathDisplay;
using Battle.BattleArena.Pathfinding;
using Battle.BattleFlow.Commands;
using RogueSharp;

namespace Battle.BattleFlow.StateMachine.MouseOverCells
{
    public class ReachableCellHoverHandler: ICellHoverHandler
    {
        private readonly BattleArenaCellsDisplayService _cellsDisplayService;
        private readonly PathfindingService _pathfindingService;
        private readonly PathDisplayService _pathDisplayService;
        private readonly BattleCellsInputService _cellsInputService;
        
        private TaskCompletionSource<ICommand> _commandCompletionSource;
        private Unit _controlledUnit;

        public ReachableCellHoverHandler(BattleArenaCellsDisplayService cellsDisplayService,
            PathfindingService pathfindingService,
            PathDisplayService pathDisplayService,
            BattleCellsInputService cellsInputService)
        {
            _cellsDisplayService = cellsDisplayService;
            _pathfindingService = pathfindingService;
            _pathDisplayService = pathDisplayService;
            _cellsInputService = cellsInputService;
        }

        public void Start(TaskCompletionSource<ICommand> commandCompletionSource, Action repaintCellsAction, Unit controlledUnit, Cell mouseoverCell)
        {
            _controlledUnit = controlledUnit;
            _commandCompletionSource = commandCompletionSource;
            
            repaintCellsAction();
            _cellsDisplayService.DisplayMoveTargetCell(mouseoverCell);
            var path = _pathfindingService.FindPath(mouseoverCell.GridPosition, controlledUnit);
            _pathDisplayService.DisplayPath(path);
            
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