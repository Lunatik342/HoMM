using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Algorithms;
using Algorithms.RogueSharp;
using Battle.AI;
using Battle.Arena.Map;
using Battle.Arena.Misc;
using Battle.CellViewsGrid.CellsViews;
using Battle.CellViewsGrid.PathDisplay;
using Battle.DamageCalculation;
using Battle.Input;
using Battle.UnitCommands.Commands;
using Battle.Units;
using UI.AttackInfoDisplayer;
using UnityEngine;

namespace Battle.CellViewsGrid.GridViewStateMachine.CellHoverHandler
{
    public class MeleeAttackCellHoverHandler: ICellHoverHandler
    {
        private readonly BattleArenaCellsDisplayService _cellsDisplayService;
        private readonly PathDisplayService _pathDisplayService;
        private readonly BattleCellsInputService _cellsInputService;
        private readonly IMapHolder _mapHolder;
        private readonly AttackPredictionInfoDisplayer _attackPredictionInfoDisplayer;
        private readonly DamagePredictionService _damagePredictionService;

        private Action _repaintCellsAction;
        private Cell _mouseoverCell;
        private List<Cell> _cellsCanAttackFrom;
        private Unit _controlledUnit;
        private TaskCompletionSource<ICommand> _commandCompletionSource;

        private Cell _cellToAttackFrom;

        public MeleeAttackCellHoverHandler(BattleArenaCellsDisplayService cellsDisplayService,
            PathDisplayService pathDisplayService,
            BattleCellsInputService cellsInputService,
            IMapHolder mapHolder,
            AttackPredictionInfoDisplayer attackPredictionInfoDisplayer,
            DamagePredictionService damagePredictionService)
        {
            _cellsDisplayService = cellsDisplayService;
            _pathDisplayService = pathDisplayService;
            _cellsInputService = cellsInputService;
            _mapHolder = mapHolder;
            _attackPredictionInfoDisplayer = attackPredictionInfoDisplayer;
            _damagePredictionService = damagePredictionService;
        }

        public void OnHover(TaskCompletionSource<ICommand> commandCompletionSource, Cell mouseoverCell, 
            Action repaintCellsAction, List<Cell> cellsCanAttackFrom, Unit controlledUnit)
        {
            _commandCompletionSource = commandCompletionSource;
            _repaintCellsAction = repaintCellsAction;
            _mouseoverCell = mouseoverCell;
            _cellsCanAttackFrom = cellsCanAttackFrom;
            _controlledUnit = controlledUnit;
            
            _cellsInputService.MousePositionChanged += MousePositionChanged;
            _cellsInputService.CellLeftClicked += OnCellClicked;

            ShowAttackPredictionView(mouseoverCell, controlledUnit);
        }

        private void ShowAttackPredictionView(Cell mouseoverCell, Unit controlledUnit)
        {
            var mouseoverUnit = mouseoverCell.PlacedUnit;
            var casualtiesPrediction = _damagePredictionService.PredictDamage(controlledUnit, mouseoverUnit);
            _attackPredictionInfoDisplayer.ShowInfo(casualtiesPrediction, mouseoverUnit.Retaliation.CanRetaliate);
        }

        private void OnCellClicked(Cell cell)
        {
            _commandCompletionSource.SetResult(
                new UnitMeleeAttackCommand(_controlledUnit, _cellToAttackFrom.GridPosition, _mouseoverCell.PlacedUnit));
        }

        private void MousePositionChanged(Vector3 mouseWorldPosition)
        {
            var attackCellCoordinate = AttackCellFromMousePositionFinder.Find(_mouseoverCell, mouseWorldPosition);
            var cell = _mapHolder.Map.GetCell(attackCellCoordinate);

            Cell cellToAttackFrom;

            if (cell != null && cell.IsWalkableByUnit(_controlledUnit) && _cellsCanAttackFrom.Contains(cell))
            {
                cellToAttackFrom = cell;
            }
            else
            {
                cellToAttackFrom = GetClosestCanAttackFromCell(attackCellCoordinate);
            }

            if (_cellToAttackFrom != cellToAttackFrom)
            {
                _cellToAttackFrom = cellToAttackFrom;
                
                _repaintCellsAction();
                _cellsDisplayService.DisplayMoveTargetCell(cellToAttackFrom);
                _cellsDisplayService.DisplayAttackTargetCell(_mouseoverCell);
                _controlledUnit.MovementController.DisplayPathToCell(_pathDisplayService, cellToAttackFrom.GridPosition);
            }
        }

        private Cell GetClosestCanAttackFromCell(Vector2Int attackCellCoordinate)
        {
            return _cellsCanAttackFrom.OrderBy(c => 
                Vector3.Distance(attackCellCoordinate.ToBattleArenaWorldPosition(), c.ToBattleArenaWorldPosition())).First();
        }

        public void Clear()
        {
            _cellsInputService.MousePositionChanged -= MousePositionChanged;
            _cellsInputService.CellLeftClicked -= OnCellClicked;
            _cellToAttackFrom = null;
            
            _pathDisplayService.StopDisplaying();
            _attackPredictionInfoDisplayer.Hide();
        }
    }
}