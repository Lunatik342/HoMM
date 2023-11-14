using Battle.StatsSystem;
using Battle.Units.Components.Interfaces;
using Battle.Units.StaticData.Components;
using UnityEngine;

namespace Battle.Units.Components
{
    public class UnitTurnsHelper: IStatsInitializer
    {
        private readonly ActingInTurnsQueueStaticData _staticData;
        private readonly UnitStatsProvider _statsProvider;
        
        private UnitStat _initiativeStat;
        private float _randomSpread;

        //To randomly place two units with the same initiative in queue
        public float InitiativeWithRandomSpread => _initiativeStat.Value + _randomSpread;
        
        public UnitTurnsHelper(ActingInTurnsQueueStaticData staticData, UnitStatsProvider statsProvider)
        {
            _staticData = staticData;
            _statsProvider = statsProvider;
        }

        public void ConfigureStats()
        {
            _initiativeStat = _statsProvider.AddStat(StatType.Initiative, _staticData.Initiative);
            _randomSpread = Random.Range(-0.01f, 0.01f);
        }
    }
}