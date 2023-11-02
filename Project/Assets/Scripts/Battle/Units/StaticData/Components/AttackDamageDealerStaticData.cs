using System;
using UnityEngine;

namespace Battle.Units.StaticData.Components
{
    [Serializable]
    public class AttackDamageDealerStaticData
    {
        [field: SerializeField] public int AttackStat { get; private set; }
        [field: SerializeField] public int DamageMin { get; private set; }
        [field: SerializeField] public int DamageMax { get; private set; }
        [field: SerializeField] public int Luck { get; private set; }
    }
}