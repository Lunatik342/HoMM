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
        
        public override void BindRelatedComponentToContainer(DiContainer container)
        {
            container.Bind(typeof(UnitMovementController), typeof(IStatsInitializer), typeof(IDeathEventReceiver))
                .To<FlyingUnitMovementController>().AsSingle().WithArguments(this);
        }
    }
}