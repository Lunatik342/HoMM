using System;
using UnityEngine;

namespace Battle.Units.StaticData.Components
{
    [Serializable]
    public class UnitGridPlaceableStaticData
    {
        [field: SerializeField] public int Size { get; private set; }
    }
}