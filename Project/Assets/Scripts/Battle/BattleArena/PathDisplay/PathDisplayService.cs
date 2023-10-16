using System.Collections.Generic;
using System.Linq;
using RogueSharp;
using UnityEngine;

namespace Battle.BattleArena.PathDisplay
{
    public class PathDisplayService
    {
        private readonly LineRenderer _pathRenderer;

        public PathDisplayService(LineRenderer pathRenderer)
        {
            _pathRenderer = pathRenderer;
        }

        public void Display(List<ICell> path)
        {
            var positions = path.Select(s =>
            {
                var worldPosition = s.GetWorldPosition();
                return new Vector3(worldPosition.x, BattleArenaObjectsHeights.PathRenderer, worldPosition.z);
            }).ToArray();
            
            _pathRenderer.positionCount = positions.Length;
            _pathRenderer.SetPositions(positions);
        }

        public void StopDisplaying()
        {
            _pathRenderer.positionCount = 0;
        }
    }
}