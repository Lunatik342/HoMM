using System;
using Battle.BattleArena;
using Battle.BattleArena.Pathfinding;
using RogueSharp;
using UnityEngine;
using Zenject;

namespace Battle.BattleFlow
{
    public class BattleCellsInputService : ITickable
    {
        private readonly Camera _mainCamera;
        private readonly IMapHolder _mapHolder;
        private Plane _floorPlane;

        private Cell _previousMouseoverCell;

        public event Action<Cell> SelectedCellChanged;
        public event Action<Cell> CellLeftClicked;
        public event Action<Cell> CellRightClicked;

        public Cell MouseOverCell => _previousMouseoverCell;

        public BattleCellsInputService(Camera mainCamera, IMapHolder mapHolder)
        {
            _mainCamera = mainCamera;
            _mapHolder = mapHolder;
            _floorPlane = new Plane(Vector3.up, Vector3.zero);
        }

        public void Tick()
        {
            Cell mouseoverCell = null;
            
            if (TryGetCellForMousePosition(out var gridPosition))
            {
                mouseoverCell = _mapHolder.Map.GetCell(gridPosition);

                if (Input.GetMouseButtonDown(0))
                {
                    CellLeftClicked?.Invoke(_previousMouseoverCell);
                }

                if (Input.GetMouseButtonDown(1))
                {
                    CellRightClicked?.Invoke(_previousMouseoverCell);
                }
            }

            if (_previousMouseoverCell != mouseoverCell)
            {
                SelectedCellChanged?.Invoke(mouseoverCell);
                _previousMouseoverCell = mouseoverCell;
            }
        }
        
        private bool TryGetCellForMousePosition(out Vector2Int gridPosition)
        {
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

            if (_floorPlane.Raycast(ray, out var enter))
            {
                Vector3 hitPoint = ray.GetPoint(enter);
                gridPosition = hitPoint.ToMapCellCoordinates();

                return IsPositionInsideGrid(gridPosition);
            }

            gridPosition = default;
            return false;
        }

        private bool IsPositionInsideGrid(Vector2Int gridPosition)
        {
            return gridPosition.x >= 0 && gridPosition.y >= 0 &&
                   _mapHolder.Map.Width > gridPosition.x && 
                   _mapHolder.Map.Height > gridPosition.y;
        }
    }
}