using Battle.BattleArena;
using Battle.BattleArena.CellsViews;
using Battle.BattleArena.Pathfinding;
using Battle.Units;
using Battle.Units.Movement;
using RogueSharp;
using UnityEngine;
using Zenject;

namespace Battle.BattleFlow
{
    public class BattleCommandsDispatcher: ITickable
    {
        private readonly BattleInputService _inputService;
        private readonly IMapHolder _mapHolder;
        private readonly UnitsMoveCommandHandler _unitsMoveCommandHandler;
        private readonly UnitsSpawner _unitsSpawner;
        private readonly BattleArenaCellsDisplayService _cellsDisplayService;
        private readonly Plane _plane;

        private Vector2Int _prevDisplayedCell;

        public bool IsEnabled { get; set; }

        public BattleCommandsDispatcher(BattleInputService inputService, 
            IMapHolder mapHolder, 
            UnitsMoveCommandHandler unitsMoveCommandHandler, 
            UnitsSpawner unitsSpawner,
            BattleArenaCellsDisplayService cellsDisplayService)
        {
            _inputService = inputService;
            _mapHolder = mapHolder;
            _unitsMoveCommandHandler = unitsMoveCommandHandler;
            _unitsSpawner = unitsSpawner;
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
                    var targetUnit = _unitsSpawner.CreatedUnits[0];
                        
                    if(_mapHolder.Map[gridPosition.x, gridPosition.y].CanPlaceEntity(targetUnit.BattleMapPlaceable));
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

                if (gridPosition.x >= 0 && gridPosition.y >= 0 &&
                    _mapHolder.Map.Width > gridPosition.x && _mapHolder.Map.Height > gridPosition.y)
                {
                    return true;
                }
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