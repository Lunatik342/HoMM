using System;
using Battle.AI.ConsiderationsFactories;
using UnityEngine;
using Zenject;

namespace Battle.AI
{
    public class AIInstaller: MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ConsiderationsFactory>().AsSingle();
            Container.Bind<MoveConsiderationsFactory>().AsSingle();
            Container.Bind<AttackConsiderationsFactory>().AsSingle();
        }
    }
}