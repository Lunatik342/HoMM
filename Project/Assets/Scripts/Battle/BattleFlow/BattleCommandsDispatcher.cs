using Battle.BattleArena;
using Battle.BattleArena.CellsViews;
using Battle.BattleArena.Pathfinding;
using Battle.Units.Movement;
using RogueSharp;
using UnityEngine;
using Zenject;

namespace Battle.BattleFlow
{
    public class BattleCommandsDispatcher: ITickable
    {
        private readonly BattleInputService _inputService;
        private readonly Map _map;
        private readonly UnitsMoveCommandHandler _unitsMoveCommandHandler;
        private readonly UnitsFactory _unitsFactory;
        private readonly BattleArenaCellsDisplayService _cellsDisplayService;
        private readonly Plane _plane;

        private Vector2Int _prevDisplayedCell;

        public bool IsEnabled { get; set; }

        public BattleCommandsDispatcher(BattleInputService inputService, 
            Map map, 
            UnitsMoveCommandHandler unitsMoveCommandHandler, 
            UnitsFactory unitsFactory,
            BattleArenaCellsDisplayService cellsDisplayService)
        {
            _inputService = inputService;
            _map = map;
            _unitsMoveCommandHandler = unitsMoveCommandHandler;
            _unitsFactory = unitsFactory;
            _cellsDisplayService = cellsDisplayService;
            IsEnabled = false;
            
            _plane = new Plane(Vector3.up, Vector3.zero);
        }
        
        public void Tick()
        {
            if (IsEnabled)
            {
                _cellsDisplayService.DisplayPrevious(_prevDisplayedCell);
                
                if (TryGetGridPosition(out var gridPosition))
                {
                    _cellsDisplayService.SetHover(gridPosition);
                    _prevDisplayedCell = gridPosition;
                }
            }
            
            if (IsEnabled && _inputService.IsMousePressed())
            {
                if (TryGetGridPosition(out var gridPosition))
                {
                    var targetUnit = _unitsFactory.CreatedUnits[0];
                        
                    if(_map[gridPosition.x, gridPosition.y].CanPlaceEntity(targetUnit.BattleMapPlaceable));
                    {
                        DispatchMoveCommand(targetUnit, gridPosition);
                    }
                }
            }
        }

        private bool TryGetGridPosition(out Vector2Int gridPosition)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (_plane.Raycast(ray, out var enter))
            {
                Vector3 hitPoint = ray.GetPoint(enter);
                gridPosition = hitPoint.ToMapCellCoordinates();

                if (gridPosition.x > 0 && gridPosition.y > 0 &&
                    _map.Width > gridPosition.x && _map.Height > gridPosition.y)
                {
                    return true;
                }
                else
                {
                    Debug.LogError($"Outside of array {gridPosition}"); 
                }
            }
            else
            {
                Debug.LogError("Raycast false");
            }

            gridPosition = Vector2Int.zero;
            return false;
        }

        private async void DispatchMoveCommand(Unit targetUnit, Vector2Int position)
        {
            IsEnabled = false;
            await _unitsMoveCommandHandler.MakeMove(targetUnit, position);
            IsEnabled = true;
        }
    }
}