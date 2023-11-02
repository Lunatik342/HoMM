using System.Collections.Generic;
using System.Threading.Tasks;
using Algorithms.RogueSharp;
using Battle.Arena.Misc;
using Battle.CellViewsGrid.CellsViews;
using Battle.CellViewsGrid.GridViewStateMachine.CellHoverHandler;
using Battle.Input;
using Battle.UnitCommands.Commands;
using Battle.Units;
using Infrastructure.SimpleStateMachine;
using Utilities;

namespace Battle.CellViewsGrid.GridViewStateMachine.States
{
    public class UnitControlViewState: IPaylodedState<UnitControlStatePayload>
    {
        private readonly BattleCellsInputService _cellsInputService;
        private readonly BattleArenaCellsDisplayService _cellsDisplayService;

        private TaskCompletionSource<ICommand> _taskCompletionSource;
        
        private List<Cell> _controlledUnitReachableCells;
        private List<Cell> _mouseoverUnitReachableCells;
        
        private Unit _controlledUnit;
        
        private readonly ReachableCellHoverHandler _reachableCellHoverHandlerHandler;
        private readonly MeleeAttackCellHoverHandler _meleeAttackCellHoverHandlerHandler;
        private readonly EmptyCellHoverHandler _emptyHoverHandler;
        
        private ICellHoverHandler _currentCellHoverHandler;

        public UnitControlViewState(BattleCellsInputService cellsInputService, 
            BattleArenaCellsDisplayService cellsDisplayService,
            ReachableCellHoverHandler reachableCellHoverHandlerHandler,
            MeleeAttackCellHoverHandler meleeAttackCellHoverHandlerHandler,
            EmptyCellHoverHandler emptyHoverHandler)
        {
            _cellsInputService = cellsInputService;
            _cellsDisplayService = cellsDisplayService;
            
            _reachableCellHoverHandlerHandler = reachableCellHoverHandlerHandler;
            _meleeAttackCellHoverHandlerHandler = meleeAttackCellHoverHandlerHandler;
            _emptyHoverHandler = emptyHoverHandler;
        }
        
        public void Enter(UnitControlStatePayload payload)
        {
            _controlledUnit = payload.Unit;
            _taskCompletionSource = payload.CommandAwaiter;
            _controlledUnitReachableCells = _controlledUnit.MovementController.GetReachableCells();

            _cellsInputService.MouseoverCellChanged += MouseoverCellChanged;
            MouseoverCellChanged(_cellsInputService.MouseOverCell);
        }

        private void MouseoverCellChanged(Cell cell)
        {
            ClearCurrentCellHoverHandler();
            _mouseoverUnitReachableCells = CellIsEmpty(cell) ? new List<Cell>() : cell.PlacedUnit.MovementController.GetReachableCells();
            
            if (CellIsReachable(cell))
            {
                _currentCellHoverHandler = _reachableCellHoverHandlerHandler;
                _reachableCellHoverHandlerHandler.Start(_taskCompletionSource, Repaint, _controlledUnit, cell);
                return;
            }
            
            if (CellHasEnemyForMeleeAttack(cell, out var canAttackFromCells))
            {
                _currentCellHoverHandler = _meleeAttackCellHoverHandlerHandler;
                _meleeAttackCellHoverHandlerHandler.Start(_taskCompletionSource, cell, Repaint, canAttackFromCells, _controlledUnit);
                return;
            }
            
            _currentCellHoverHandler = _emptyHoverHandler;
            _emptyHoverHandler.Start(Repaint);
        }

        private bool CellIsEmpty(Cell cell) => cell?.PlacedUnit == null || cell.PlacedUnit == _controlledUnit;
        private bool CellIsReachable(Cell cell) => cell != null && _controlledUnitReachableCells.Contains(cell);
        private bool CellHasEnemyForMeleeAttack(Cell cell, out List<Cell> canAttackFromCells)
        {
            canAttackFromCells = null;
            return cell?.PlacedUnit != null && 
                   cell.PlacedUnit.Team != _controlledUnit.Team &&
                   CanReachCellForMeleeAttack(cell, _controlledUnitReachableCells, out canAttackFromCells);
        }

        private void Repaint()
        {
            _cellsDisplayService.DisplayAllCellsDefault();
            _cellsDisplayService.DisplayReachableCells(_controlledUnitReachableCells, _mouseoverUnitReachableCells);
            _cellsDisplayService.DisplayCurrentlyControlledUnitCell(_controlledUnit.PositionProvider.OccupiedCell);
        }

        private bool CanReachCellForMeleeAttack(Cell cell, List<Cell> reachableCells, out List<Cell> adjacentToEnemyCells)
        {
            var tolerance = 0.01f;
            adjacentToEnemyCells = new List<Cell>();
            
            foreach (var reachableCell in reachableCells)
            {
                if (CellsUtilities.CalculateDistance(reachableCell, cell) < 
                    BattleArenaConstants.DiagonalMovementCost + tolerance)
                {
                    adjacentToEnemyCells.Add(reachableCell);
                }
            }

            return adjacentToEnemyCells.Count > 0;
        }

        public void Exit()
        {
            _cellsInputService.MouseoverCellChanged -= MouseoverCellChanged;
            ClearCurrentCellHoverHandler();
        }

        private void ClearCurrentCellHoverHandler()
        {
            _currentCellHoverHandler?.Clear();
            _currentCellHoverHandler = null;
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