using System.Collections.Generic;
using Battle.StatsSystem;
using Battle.Units.Components.Interfaces;
using Battle.Units.StaticData.Components;
using UnityEngine;
using Utilities;

namespace Battle.Units.Components
{
    public class UnitTurnsHelper: IStatsInitializer
    {
        private readonly ActingInTurnsQueueStaticData _staticData;
        private readonly UnitStatsProvider _statsProvider;
        private readonly List<ITurnEndEventReceiver> _turnEndEventReceivers;

        private UnitStat _initiativeStat;
        private float _randomSpread;

        //To randomly place two units with the same initiative in queue
        public float InitiativeWithRandomSpread => _initiativeStat.Value + _randomSpread;
        
        public UnitTurnsHelper(ActingInTurnsQueueStaticData staticData, UnitStatsProvider statsProvider, List<ITurnEndEventReceiver> turnEndEventReceivers)
        {
            _staticData = staticData;
            _statsProvider = statsProvider;
            _turnEndEventReceivers = turnEndEventReceivers;
        }

        public void NotifyTurnEnd()
        {
            _turnEndEventReceivers.Foreach(t => t.OnTurnEnd());
        }

        void IStatsInitializer.ConfigureStats()
        {
            _initiativeStat = _statsProvider.AddStat(StatType.Initiative, _staticData.Initiative);
            _randomSpread = Random.Range(-0.01f, 0.01f);
        }
    }
}