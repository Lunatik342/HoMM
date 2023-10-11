using System;
using UnityEngine;

namespace Battle.Units.Movement.StaticData
{
    [CreateAssetMenu(fileName = "GroundUnitMovementStaticData", menuName = "StaticData/Units/Movement/GroundUnitMovementStaticData")]
    public class GroundUnitMovementStaticData : ScriptableObject, IUnitComponentBinder
    {
        [field: SerializeField] public float MovementSpeed { get; private set; }


        public Type GetContractTypeToBind()
        {
            return typeof(GroundUnitMovementController);
        }

        public object GetArgument()
        {
            return this;
        }
    }
}