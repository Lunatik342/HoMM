using System;
using System.Collections.Generic;
using Battle.Arena.Obstacles;
using Battle.Arena.StaticData;
using Battle.BattleFlow;
using Battle.BattleFlow.Phases;
using Battle.UnitCommands.Providers;
using Battle.Units.StaticData;
using Infrastructure.SimpleStateMachine;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Battle
{
    public class BattleInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<BattleBootstrapper>().AsSingle().NonLazy();
            Container.Bind<BattlePhasesStateMachine>().AsSingle();
            Container.Bind<StatesFactory>().AsSingle();
        }
    }
}
