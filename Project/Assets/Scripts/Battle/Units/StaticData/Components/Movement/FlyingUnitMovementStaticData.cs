using System;
using UnityEngine;
using Zenject;

namespace Battle.Units.Movement.StaticData
{
    [CreateAssetMenu(fileName = "FlyingUnitMovementStaticData", menuName = "StaticData/Units/Movement/FlyingUnitMovementData")]
    public class FlyingUnitMovementStaticData : MovementStaticData
    {
        [field: SerializeField] public float FlySpeed { get; private set; }
        [field: SerializeField] public float JumpPower { get; private set; }
        
        public override void BindComponentToContainer(DiContainer container)
        {
            container.Bind(typeof(UnitMovementControllerBase), typeof(IUnitInitializable))
                .To<FlyingUnitMovementController>().AsSingle().WithArguments(this);
        }
    }
}