using Battle.StatsSystem;
using Battle.Units.Components;
using Battle.Units.StaticData;
using Battle.Units.WorldUI;
using UnityEngine;
using Zenject;

namespace Battle.Units.Creation
{
    public class UnitComponentsInstaller : Installer<UnitComponentsInstaller>
    {
        private readonly GameObject _gameObject;
        private readonly Team _team;
        private readonly UnitStaticData _staticData;
        private readonly UnitHealthView _healthViewPrefab;

        public UnitComponentsInstaller(GameObject gameObject, Team team, UnitStaticData staticData, UnitHealthView healthViewPrefab)
        {
            _gameObject = gameObject;
            _team = team;
            _staticData = staticData;
            _healthViewPrefab = healthViewPrefab;
        }
        
        public override void InstallBindings()
        {
            BindGameObjectComponents();
            BindHealthView();
            BindLogicalComponents();

            Container.BindInstance(_team);
            Container.BindInstance(_staticData.UnitId);

            Container.Bind<Unit>().AsSingle();
            Container.Bind<UnitInitializer>().AsSingle();
            Container.Bind<UnitDeathHandler>().AsSingle().NonLazy();
        }

        private void BindGameObjectComponents()
        {
            Container.QueueForInject(_gameObject);

            Container.Bind<UnitAnimator>().FromComponentOn(_gameObject).AsSingle();
            Container.Bind<Transform>().FromComponentOn(_gameObject).AsSingle();
            Container.BindInstance(_gameObject);
        }

        private void BindHealthView()
        {
            Container.BindInterfacesAndSelfTo<UnitHealthView>().FromComponentInNewPrefab(_healthViewPrefab)
                .UnderTransform(_gameObject.transform).AsSingle().NonLazy();
        }

        private void BindLogicalComponents()
        {
            Container.BindInterfacesAndSelfTo<RotationController>().AsSingle().WithArguments(_staticData.UnitRotationStaticData);
            Container.BindInterfacesAndSelfTo<BattleMapPlaceable>().AsSingle().WithArguments(_staticData.UnitGridPlaceableStaticData);
            Container.BindInterfacesAndSelfTo<UnitHealth>().AsSingle().WithArguments(_staticData.DamageReceiverStaticData);
            Container.Bind<UnitStatsProvider>().AsSingle();
            Container.Bind<UnitSimpleActions>().AsSingle();
            Container.BindInterfacesAndSelfTo<UnitTurnsNotificationsReceiver>().AsSingle().WithArguments(_staticData.ActingInTurnsQueueStaticData);
            Container.BindInterfacesAndSelfTo<UnitAttack>().AsSingle().WithArguments(_staticData.AttackDamageStaticData);
            Container.BindInterfacesAndSelfTo<UnitRetaliation>().AsSingle();

            _staticData.MovementStaticData.BindRelatedComponentToContainer(Container);
        }
    }
}