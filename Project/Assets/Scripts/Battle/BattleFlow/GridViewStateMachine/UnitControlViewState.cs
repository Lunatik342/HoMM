using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Battle.BattleArena.CellsViews;
using Battle.BattleArena.PathDisplay;
using Battle.BattleArena.Pathfinding;
using Battle.BattleFlow.Commands;
using Infrastructure.SimpleStateMachine;
using RogueSharp;
using UnityEngine;

namespace Battle.BattleFlow.StateMachine
{
    public class UnitControlViewState: IPaylodedState<UnitControlStatePayload>
    {
        private readonly BattleCellsInputService _cellsInputService;
        private readonly BattleArenaCellsDisplayService _cellsDisplayService;
        private readonly PathfindingService _pathfindingService;
        private readonly PathDisplayService _pathDisplayService;

        private TaskCompletionSource<ICommand> _taskCompletionSource;
        private List<Cell> _reachableCells;
        private Unit _unit;
        
        public UnitControlViewState(BattleCellsInputService cellsInputService, 
            BattleArenaCellsDisplayService cellsDisplayService,
            PathfindingService pathfindingService,
            PathDisplayService pathDisplayService)
        {
            _cellsInputService = cellsInputService;
            _cellsDisplayService = cellsDisplayService;
            _pathfindingService = pathfindingService;
            _pathDisplayService = pathDisplayService;
        }
        
        public void Enter(UnitControlStatePayload payload)
        {
            _unit = payload.Unit;
            _taskCompletionSource = payload.CommandAwaiter;
            
            _reachableCells = _pathfindingService.GetReachableCells(payload.Unit);

            _cellsInputService.SelectedCellChanged += MouseOverCellChanged;
            _cellsInputService.CellLeftClicked += OnCellClicked;

            MouseOverCellChanged(_cellsInputService.MouseOverCell);
        }
        
        private void OnCellClicked(Cell clickedCell)
        {
            if (_reachableCells.Contains(clickedCell))
            {
                _pathDisplayService.StopDisplaying();
                _cellsDisplayService.DisplayAllCellsDefault();
                _taskCompletionSource.SetResult(new UnitMoveCommand(_unit, new Vector2Int(clickedCell.X, clickedCell.Y)));
            }
        }

        private void MouseOverCellChanged(Cell cell)
        {
            _cellsDisplayService.DisplayAllCellsDefault();
            _cellsDisplayService.DisplayReachableCells(_reachableCells);
            _cellsDisplayService.DisplayCurrentlyControlledUnitCell(_unit.BattleMapPlaceable.OccupiedCell);
            
            if (_reachableCells.Contains(cell))
            {
                _cellsDisplayService.DisplayMoveTargetCell(cell);
                var path = _pathfindingService.FindPath(cell.GridPosition, _unit);
                _pathDisplayService.Display(path.Steps.ToList());
            }
            else
            {
                _pathDisplayService.StopDisplaying();
            }
        }

        public void Exit()
        {
            _cellsInputService.SelectedCellChanged -= MouseOverCellChanged;
            _cellsInputService.CellLeftClicked -= OnCellClicked;
        }
    }

    public class UnitControlStatePayload
    {
        public Unit Unit { get; }
        public TaskCompletionSource<ICommand> CommandAwaiter { get; }

        public UnitControlStatePayload(TaskCompletionSource<ICommand> commandAwaiter, Unit unit)
        {
            CommandAwaiter = commandAwaiter;
            Unit = unit;
        }
    }
}