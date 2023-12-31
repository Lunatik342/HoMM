using System;
using UnityEngine;

namespace Battle.Units.StaticData.Components
{
    [Serializable]
    public class DamageReceiverStaticData
    {
        [field: SerializeField] public int MaxHealth { get; private set; }
        [field: SerializeField] public int Defence { get; private set; }
    }
}