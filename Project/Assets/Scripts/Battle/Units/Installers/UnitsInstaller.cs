using System.Collections.Generic;
using Battle.Units.Creation;
using Battle.Units.StaticData;
using UnityEngine;
using Zenject;

namespace Battle.Units.Installers
{
    public class UnitsInstaller: MonoInstaller<UnitsInstaller>
    {
        [SerializeField] private List<UnitStaticData> _unitStaticData;

        public override void InstallBindings()
        {
            Container.Bind<UnitsStaticDataProvider>().AsSingle().WithArguments(_unitStaticData);
            
            Container.BindFactory<GameObject, Team, UnitStaticData, Unit, Unit.Factory>().FromSubContainerResolve().ByInstaller<UnitComponentsInstaller>();
            Container.Bind<UnitSpawner>().AsSingle();
            Container.BindInterfacesAndSelfTo<ArmySpawner>().AsSingle();
        }
    }
}