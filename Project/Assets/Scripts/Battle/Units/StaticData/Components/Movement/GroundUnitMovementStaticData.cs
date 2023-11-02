using Battle.Units.Components.Interfaces;
using Battle.Units.Components.Movement;
using UnityEngine;
using Zenject;

namespace Battle.Units.StaticData.Components.Movement
{
    [CreateAssetMenu(fileName = "GroundUnitMovementStaticData", menuName = "StaticData/Units/Movement/GroundUnitMovementStaticData")]
    public class GroundUnitMovementStaticData : MovementStaticData
    {
        [field: SerializeField] public float MovementSpeed { get; private set; }
        
        public override void BindRelatedComponentToContainer(DiContainer container)
        {
            container.Bind(typeof(UnitMovementController), typeof(IStatsInitializer), typeof(IDeathEventReceiver))
                .To<GroundUnitMovementController>().AsSingle().WithArguments(this);
        }
    }
}