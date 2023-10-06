using System.Collections.Generic;
using Battle.BattleArena.Pathfinding.StaticData;
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
            Container.Bind<UnitsStaticDataProvider>().AsSingle();
            Container.BindInstance(_unitStaticData);

            Container.Bind<UnitsFactory>().AsSingle();
            Container.Bind<UnitsMoveCommandHandler>().AsSingle();
        }
    }
}