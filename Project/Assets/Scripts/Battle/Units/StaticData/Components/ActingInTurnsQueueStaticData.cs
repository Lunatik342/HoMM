using System;
using UnityEngine;

namespace Battle.BattleArena.Pathfinding.StaticData
{
    [Serializable]
    public class ActingInTurnsQueueStaticData
    {
        [field: SerializeField] public int Initiative { get; private set; }
        [field: SerializeField] public int Morale { get; private set; }
    }
}