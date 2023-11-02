using System;
using UnityEngine;

namespace Battle.Units.StaticData.Components
{
    [Serializable]
    public class UnitRotationStaticData
    {
        [field: SerializeField] public float RotationSpeed { get; private set; }
    }
}