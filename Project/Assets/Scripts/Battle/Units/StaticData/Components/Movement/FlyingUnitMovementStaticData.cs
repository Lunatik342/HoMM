using System;
using UnityEngine;

namespace Battle.Units.Movement.StaticData
{
    [CreateAssetMenu(fileName = "FlyingUnitMovementStaticData", menuName = "StaticData/Units/Movement/FlyingUnitMovementData")]
    public class FlyingUnitMovementStaticData : ScriptableObject, IUnitComponentBinder
    {
        [field: SerializeField] public float FlySpeed { get; private set; }
        [field: SerializeField] public float JumpPower { get; private set; }

        public Type GetContractTypeToBind()
        {
            return typeof(FlyingUnitMovementController);
        }

        public object GetArgument()
        {
            return this;
        }
    }
}