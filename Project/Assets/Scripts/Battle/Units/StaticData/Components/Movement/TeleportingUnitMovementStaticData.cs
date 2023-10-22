using UnityEngine;
using Zenject;

namespace Battle.Units.Movement.StaticData
{
    [CreateAssetMenu(fileName = "TeleportingUnitMovementStaticData", menuName = "StaticData/Units/Movement/TeleportingUnitMovementStaticData")]
    public class TeleportingUnitMovementStaticData : MovementStaticData
    {
        [field: SerializeField] public float DelayBeforeTeleport { get; private set; }
        [field: SerializeField] public float DelayAfterTeleport { get; private set; }
        
        public override void BindComponentToContainer(DiContainer container)
        {
            container.Bind(typeof(UnitMovementControllerBase), typeof(IUnitInitializable), typeof(IDeathEventReceiver)).
                To<TeleportingUnitMovementController>().AsSingle().WithArguments(this);
        }
    }
}