using Battle.BattleArena.Pathfinding;
using Battle.BattleArena.Pathfinding.StaticData;
using Battle.Units.Animation;
using Battle.Units.Components;
using Battle.Units.Movement;
using Battle.Units.StatsSystem;
using UnityEngine;
using Zenject;

namespace Battle.Units
{
    public class UnitComponentsInstaller : Installer<UnitComponentsInstaller>
    {
        private readonly GameObject _gameObject;
        private readonly Team _team;
        private readonly UnitStaticData _staticData;

        public UnitComponentsInstaller(GameObject gameObject, Team team, UnitStaticData staticData)
        {
            _gameObject = gameObject;
            _team = team;
            _staticData = staticData;
        }
        
        public override void InstallBindings()
        {
            Container.QueueForInject(_gameObject);
            
            Container.Bind<UnitAnimator>().FromComponentOn(_gameObject).AsSingle();
            Container.Bind<Transform>().FromComponentOn(_gameObject).AsSingle();
            Container.BindInstance(_gameObject);
            
            Container.BindInstance(_team);
            Container.Bind<Unit>().AsSingle();
            Container.Bind<RotationController>().AsSingle().WithArguments(_staticData.UnitRotationStaticData);
            Container.Bind<BattleMapPlaceable>().AsSingle().WithArguments(_staticData.UnitGridPlaceableStaticData);
            Container.BindInterfacesAndSelfTo<UnitHealth>().AsSingle().WithArguments(_staticData.DamageReceiverStaticData);
            Container.Bind<UnitStatsProvider>().AsSingle();
            Container.Bind<UnitActions>().AsSingle();
            Container.BindInterfacesAndSelfTo<UnitAttack>().AsSingle().WithArguments(_staticData.AttackDamageStaticData);
            Container.BindInterfacesAndSelfTo<UnitHealthView>().FromComponentInNewPrefabResource("UnitCanvas")
                .UnderTransform(_gameObject.transform).AsSingle().NonLazy();

            _staticData.MovementStaticData.BindRelatedComponentToContainer(Container);
        }
    }
}