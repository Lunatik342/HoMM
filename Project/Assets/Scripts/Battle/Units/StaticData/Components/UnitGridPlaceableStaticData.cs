using System;
using UnityEngine;

namespace Battle.BattleArena.Pathfinding.StaticData
{
    [Serializable]
    public class UnitGridPlaceableStaticData
    {
        [field: SerializeField] public int Size { get; private set; }
    }
}