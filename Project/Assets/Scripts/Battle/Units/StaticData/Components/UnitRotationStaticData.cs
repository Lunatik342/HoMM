using System;
using UnityEngine;

namespace Battle.BattleArena.Pathfinding.StaticData
{
    [Serializable]
    public class UnitRotationStaticData
    {
        [field: SerializeField] public float RotationSpeed { get; private set; }
    }
}