using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Battle.BattleField
{
    public class BattleFieldInstaller : MonoInstaller
    {
        [SerializeField] private List<BattleFieldStaticData> _battleFieldStaticData;

        public override void InstallBindings()
        {
            Container.Bind<List<BattleFieldStaticData>>().FromInstance(_battleFieldStaticData);
            Container.Bind<BattleFieldStaticDataService>().AsSingle();
            Container.Bind<BattleFieldSpawner>().AsSingle();
        }
    }
}
