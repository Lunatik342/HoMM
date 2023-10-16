using System.Collections.Generic;
using Battle.BattleArena.Pathfinding.StaticData;
using Battle.Units;
using Battle.Units.Movement;
using UnityEngine;
using Zenject;

namespace Battle.BattleArena.Pathfinding.Installers
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