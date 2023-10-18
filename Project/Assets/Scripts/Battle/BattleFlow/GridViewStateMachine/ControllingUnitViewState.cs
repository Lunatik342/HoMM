using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Battle.BattleArena;
using Battle.BattleArena.CellsViews;
using Battle.BattleArena.PathDisplay;
using Battle.BattleArena.Pathfinding;
using Battle.BattleFlow.Commands;
using Infrastructure.SimpleStateMachine;
using RogueSharp;
using RogueSharp.Algorithms;
using UnityEngine;

namespace Battle.BattleFlow.StateMachine
{
    public class ControllingUnitViewState: IPaylodedState<ControllingUnitStatePayload>
    {
        private readonly BattleCellsInputService _cellsInputService;
        private readonly IMapHolder _mapHolder;
        private readonly BattleArenaCellsDisplayService _cellsDisplayService;
        private readonly PathfindingService _pathfindingService;
        private readonly PathDisplayService _pathDisplayService;

        private TaskCompletionSource<ICommand> _taskCompletionSource;
        private List<Cell> _reachableCells;
        private Unit _unit;
        
        public ControllingUnitViewState(BattleCellsInputService cellsInputService, 
            IMapHolder mapHolder, 
            BattleArenaCellsDisplayService cellsDisplayService,
            PathfindingService pathfindingService,
            PathDisplayService pathDisplayService)
        {
            _cellsInputService = cellsInputService;
            _mapHolder = mapHolder;
            _cellsDisplayService = cellsDisplayService;
            _pathfindingService = pathfindingService;
            _pathDisplayService = pathDisplayService;
        }
        
        public void Enter(ControllingUnitStatePayload payload)
        {
            _unit = payload.Unit;
            _taskCompletionSource = payload.CommandAwaiter;
            
            _reachableCells = GetReachableCells(payload.Unit);

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
                var path = _pathfindingService.FindPath(cell.GridPosition, _unit.BattleMapPlaceable);
                _pathDisplayService.Display(path.Steps.ToList());
            }
            else
            {
                _pathDisplayService.StopDisplaying();
            }
        }

        private List<Cell> GetReachableCells(Unit unit)
        {
            var occupiedCell = unit.BattleMapPlaceable.OccupiedCell;
            var reachableCellsFinder = new ReachableCellsFinder<Cell>(BattleArenaConstants.DiagonalMovementCost);
            var cellsPositions = reachableCellsFinder.GetReachableCells(occupiedCell, _mapHolder.Map, 
                unit.BattleMapPlaceable, 6);

            List<Cell> reachableCells = new List<Cell>();

            for (int i = 0; i < cellsPositions.Length; i++)
            {
                if (cellsPositions[i])
                {
                    var cell = _mapHolder.Map.CellFor(i);
                    reachableCells.Add(cell);
                }
            }

            return reachableCells;
        }

        public void Exit()
        {
            _cellsInputService.SelectedCellChanged -= MouseOverCellChanged;
            _cellsInputService.CellLeftClicked -= OnCellClicked;
        }
    }

    public class ControllingUnitStatePayload
    {
        public Unit Unit { get; private set; }
        public TaskCompletionSource<ICommand> CommandAwaiter { get; private set; }

        public ControllingUnitStatePayload(TaskCompletionSource<ICommand> commandAwaiter, Unit unit)
        {
            CommandAwaiter = commandAwaiter;
            Unit = unit;
        }
    }
}