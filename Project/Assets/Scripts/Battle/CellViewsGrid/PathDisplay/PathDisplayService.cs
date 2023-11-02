using System.Collections.Generic;
using System.Linq;
using Algorithms.RogueSharp;
using Battle.Arena.Misc;
using UnityEngine;

namespace Battle.CellViewsGrid.PathDisplay
{
    public class PathDisplayService
    {
        private readonly LineRenderer _pathRenderer;

        public PathDisplayService(LineRenderer pathRenderer)
        {
            _pathRenderer = pathRenderer;
        }

        public void DisplayPath(List<ICell> path)
        {
            var positions = path.Select(s =>
            {
                var worldPosition = s.GetWorldPosition();
                return new Vector3(worldPosition.x, BattleArenaObjectsHeights.PathRenderer, worldPosition.z);
            }).ToArray();
            
            _pathRenderer.positionCount = positions.Length;
            _pathRenderer.SetPositions(positions);
        }

        public void DisplayArc(List<ICell> path)
        {
            List<Vector3> arcPoints = new List<Vector3>();

            var point1 = path[0].GetWorldPosition();
            var point2 = path[^1].GetWorldPosition();

            for (float i = 0; i <= 1; i += 0.05f)
            {
                arcPoints.Add(SampleParabola(point1, point2, 1, i));
            }
            
            _pathRenderer.positionCount = arcPoints.Count;
            _pathRenderer.SetPositions(arcPoints.ToArray());
        }

        public void StopDisplaying()
        {
            _pathRenderer.positionCount = 0;
        }

        private Vector3 SampleParabola(Vector3 start, Vector3 end, float height, float t)
        {
            float parabolicT = t * 2 - 1;
            Vector3 travelDirection = end - start;
            Vector3 result = start + t * travelDirection;
            result += (-parabolicT * parabolicT + 1) * height * Vector3.up;
            return result;
        }
    }
}