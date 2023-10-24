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
        private Vector3 _previousMousePosition;

        public event Action<Cell> MouseoverCellChanged;
        public event Action<Cell> CellLeftClicked;
        public event Action<Cell> CellRightClicked;
        public event Action<Vector3> MousePositionChanged;

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
            
            if (TryGetCellForMousePosition(out var gridPosition, out var hitPosition))
            {
                mouseoverCell = _mapHolder.Map.GetCell(gridPosition);

                if (Input.GetMouseButtonDown(0))
                {
                    CellLeftClicked?.Invoke(mouseoverCell);
                }

                if (Input.GetMouseButtonDown(1))
                {
                    CellRightClicked?.Invoke(mouseoverCell);
                }
            }
            
            if (_previousMouseoverCell != mouseoverCell)
            {
                MouseoverCellChanged?.Invoke(mouseoverCell);
                _previousMouseoverCell = mouseoverCell;
            }

            if (_previousMousePosition != hitPosition)
            {
                MousePositionChanged?.Invoke(hitPosition);
                _previousMousePosition = hitPosition;
            }
        }

        private bool TryGetCellForMousePosition(out Vector2Int gridPosition, out Vector3 hitPoint)
        {
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

            if (_floorPlane.Raycast(ray, out var enter))
            {
                hitPoint = ray.GetPoint(enter);
                gridPosition = hitPoint.ToMapCellCoordinates();

                return IsPositionInsideGrid(gridPosition);
            }

            gridPosition = default;
            hitPoint = default;
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