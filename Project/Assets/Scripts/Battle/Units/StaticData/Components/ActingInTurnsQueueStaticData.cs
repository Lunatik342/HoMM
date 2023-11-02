using System;
using UnityEngine;

namespace Battle.Units.StaticData.Components
{
    [Serializable]
    public class ActingInTurnsQueueStaticData
    {
        [field: SerializeField] public int Initiative { get; private set; }
        [field: SerializeField] public int Morale { get; private set; }
    }
}