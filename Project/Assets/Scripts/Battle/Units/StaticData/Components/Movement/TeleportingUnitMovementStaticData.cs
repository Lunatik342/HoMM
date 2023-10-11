using System;
using UnityEngine;

namespace Battle.Units.Movement.StaticData
{
    [CreateAssetMenu(fileName = "TeleportingUnitMovementStaticData", menuName = "StaticData/Units/Movement/TeleportingUnitMovementStaticData")]
    public class TeleportingUnitMovementStaticData : ScriptableObject, IUnitComponentBinder
    {
        [field: SerializeField] public float DelayBeforeTeleport { get; private set; }
        [field: SerializeField] public float DelayAfterTeleport { get; private set; }


        public Type GetContractTypeToBind()
        {
            return typeof(TeleportingUnitMovementController);
        }

        public object GetArgument()
        {
            return this;
        }
    }
}