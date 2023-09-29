using Battle.BattleField.Pathfinding;
using RogueSharp;

namespace Battle.BattleField
{
    public class RandomObstaclesFactory
    {
        private readonly PathfindingMapFactory _pathfindingMapFactory;

        public RandomObstaclesFactory(PathfindingMapFactory pathfindingMapFactory)
        {
            _pathfindingMapFactory = pathfindingMapFactory;
        }

        public void SpawnRandomObstacles()
        {
            _pathfindingMapFactory.SetCellWalkable(false,6 ,0);
            _pathfindingMapFactory.SetCellWalkable(false,6 ,1);
            _pathfindingMapFactory.SetCellWalkable(false,6 ,2);
        }
    }
}