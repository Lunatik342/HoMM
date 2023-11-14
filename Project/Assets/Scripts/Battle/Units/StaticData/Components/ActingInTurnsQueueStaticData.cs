using System;
using UnityEngine;

namespace Battle.Units.StaticData.Components
{
    [Serializable]
    public class ActingInTurnsQueueStaticData
    {
        [field: SerializeField] public int Initiative { get; private set; }
    }
}