using System;
using Battle.BattleArena.Pathfinding;
using Battle.BattleArena.Pathfinding.StaticData;
using Battle.Units.Movement;
using UnityEngine;
using Zenject;

namespace Battle.Units
{
    public class UnitInstaller : Installer<UnitInstaller>
    {
        private readonly UnitId _unitId;
        private readonly GameObject _gameObject;
        private readonly MovementType _movementType;

        public UnitInstaller(UnitId unitId, GameObject gameObject, MovementType movementType)
        {
            _unitId = unitId;
            _gameObject = gameObject;
            _movementType = movementType;
        }
        
        public override void InstallBindings()
        {
            Container.BindInstance(_gameObject.transform);
            Container.BindInstance(_gameObject);
            
            Container.Bind<Unit>().AsSingle();
            Container.Bind<RotationController>().AsSingle().WithArguments(new object[]{360f, Vector3.right});
            Container.Bind<BattleMapPlaceable>().AsSingle();

            switch (_movementType)
            {
                case MovementType.None:
                    break;
                case MovementType.OnGround:
                    Container.Bind<IUnitMovementController>().To<GroundUnitMovementController>().AsSingle().WithArguments(new object[]{5f});
                    break;
                case MovementType.Flying:
                    Container.Bind<IUnitMovementController>().To<FlyingUnitMovementController>().AsSingle().WithArguments(new object[]{5f});
                    break;
                case MovementType.Teleporting:
                    Container.Bind<IUnitMovementController>().To<TeleportingUnitMovementController>().AsSingle();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}