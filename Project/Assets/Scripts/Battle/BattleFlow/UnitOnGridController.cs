using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Battle.BattleArena;
using Battle.BattleArena.CellsViews;
using Battle.BattleArena.PathDisplay;
using Battle.BattleArena.Pathfinding;
using Battle.Units.Movement;
using Cysharp.Threading.Tasks;
using RogueSharp;
using RogueSharp.Algorithms;
using UnityEngine;

namespace Battle.BattleFlow
{
    public class UnitOnGridController
    {
        private readonly BattleCellsInputService _cellsInputService;
        private readonly IMapHolder _mapHolder;
        private readonly BattleArenaCellsDisplayService _cellsDisplayService;
        private readonly UnitsMoveCommandHandler _moveCommandHandler;
        private readonly PathfindingService _pathfindingService;
        private readonly PathDisplayService _pathDisplayService;

        private TaskCompletionSource<bool> _taskCompletionSource;
        private List<Cell> _reachableCells;
        private Unit _unit;

        public UnitOnGridController(BattleCellsInputService cellsInputService, 
            IMapHolder mapHolder, 
            BattleArenaCellsDisplayService cellsDisplayService,
            UnitsMoveCommandHandler moveCommandHandler,
            PathfindingService pathfindingService,
            PathDisplayService pathDisplayService)
        {
            _cellsInputService = cellsInputService;
            _mapHolder = mapHolder;
            _cellsDisplayService = cellsDisplayService;
            _moveCommandHandler = moveCommandHandler;
            _pathfindingService = pathfindingService;
            _pathDisplayService = pathDisplayService;
        }

        public async UniTask Do(Unit unit)
        {
            _unit = unit;
            _reachableCells = GetReachableCells(unit);
            _cellsInputService.SelectedCellChanged += MouseOverCellChanged;
            _cellsInputService.CellLeftClicked += OnCellClicked;
            MouseOverCellChanged(_cellsInputService.MouseOverCell);

            _taskCompletionSource = new TaskCompletionSource<bool>();
            await _taskCompletionSource.Task;
        }

        private async void OnCellClicked(Cell clickedCell)
        {
            if (_reachableCells.Contains(clickedCell))
            {
                _pathDisplayService.StopDisplaying();
                _cellsDisplayService.DisplayAllCellsDefault();
                await _moveCommandHandler.MakeMove(_unit, new Vector2Int(clickedCell.X, clickedCell.Y));
                _taskCompletionSource.SetResult(true);
            }
        }

        private void MouseOverCellChanged(Cell cell)
        {
            _cellsDisplayService.DisplayAllCellsDefault();
            _cellsDisplayService.DisplayReachableCells(_reachableCells);
            
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
            //Пока нет многоячеичных персов, будет так
            var occupiedCell = unit.BattleMapPlaceable.OccupiedCells[0];
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

        public void StopDoing()
        {
            _cellsInputService.SelectedCellChanged -= MouseOverCellChanged;
            _cellsInputService.CellLeftClicked -= OnCellClicked;
        }
    }
}