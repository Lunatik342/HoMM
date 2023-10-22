using System;
using UnityEngine;
using Zenject;

namespace Battle.Units.Movement.StaticData
{
    [CreateAssetMenu(fileName = "GroundUnitMovementStaticData", menuName = "StaticData/Units/Movement/GroundUnitMovementStaticData")]
    public class GroundUnitMovementStaticData : MovementStaticData
    {
        [field: SerializeField] public float MovementSpeed { get; private set; }
        
        public override void BindComponentToContainer(DiContainer container)
        {
            container.Bind(typeof(UnitMovementControllerBase), typeof(IUnitInitializable), typeof(IDeathEventReceiver))
                .To<GroundUnitMovementController>().AsSingle().WithArguments(this);
        }
    }
}