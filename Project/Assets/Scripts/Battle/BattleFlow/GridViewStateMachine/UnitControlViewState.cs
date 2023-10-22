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
    public class UnitControlViewState: DisplayingCellsViewStateBase, IPaylodedState<UnitControlStatePayload>
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
            PathDisplayService pathDisplayService): base(cellsDisplayService)
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
            _reachableCells = _unit.MovementController.GetReachableCells();

            _cellsInputService.MouseoverCellChanged += MouseoverCellChanged;
            _cellsInputService.CellLeftClicked += OnCellClicked;

            MouseoverCellChanged(_cellsInputService.MouseOverCell);
        }
        
        private void OnCellClicked(Cell clickedCell)
        {
            if (_reachableCells.Contains(clickedCell))
            {
                _pathDisplayService.StopDisplaying();
                _taskCompletionSource.SetResult(new UnitMoveCommand(_unit, new Vector2Int(clickedCell.X, clickedCell.Y)));
            }
        }

        private void MouseoverCellChanged(Cell cell)
        {
            _cellsDisplayService.DisplayAllCellsDefault();
            DisplayReachableCells(cell, _reachableCells);
            _cellsDisplayService.DisplayCurrentlyControlledUnitCell(_unit.BattleMapPlaceable.OccupiedCell);
            DisplayPathToMouseOverCell(cell);
        }

        private void DisplayPathToMouseOverCell(Cell cell)
        {
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
            _cellsInputService.MouseoverCellChanged -= MouseoverCellChanged;
            _cellsInputService.CellLeftClicked -= OnCellClicked;
        }
    }
 }