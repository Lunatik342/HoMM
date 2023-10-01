using System.Collections.Generic;
using System.Linq;

namespace Battle.BattleArena.StaticData
{
    public class BattleArenaStaticDataProvider
    {
        private readonly Dictionary<BattleArenaId, BattleArenaStaticData> _battleArenasData;
        private readonly Dictionary<ObstacleId, ObstacleStaticData> _obstaclesData;

        public BattleArenaStaticDataProvider(BattleArenaStaticData[] battleArenas, ObstacleStaticData[] obstacles)
        {
            _battleArenasData = battleArenas.ToDictionary(s => s.Id);
            _obstaclesData = obstacles.ToDictionary(s => s.Id);
        }

        public BattleArenaStaticData ForBattleArena(BattleArenaId id)
        {
            return _battleArenasData[id];
        }

        public ObstacleStaticData ForObstacle(ObstacleId id)
        {
            return _obstaclesData[id];
        }

        public IEnumerable<ObstacleStaticData> GetObstaclesForBattleArena(BattleArenaId id)
        {
            var battleArena = _battleArenasData[id];

            foreach (var obstacleId in battleArena.PossibleObstacles)
            {
                yield return ForObstacle(obstacleId);
            }
        }
    }
}