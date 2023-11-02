using Battle.Input;
using UnityEngine;
using Zenject;

namespace Battle.BattleFlow.Installers
{
    public class BattleFlowInstaller: MonoInstaller<BattleFlowInstaller>
    {
        [SerializeField] private Camera _mainCamera;
        
        public override void InstallBindings()
        {
            Container.BindInstance(_mainCamera).AsSingle();
            Container.BindInterfacesAndSelfTo<BattleCellsInputService>().AsSingle();
            
            Container.Bind<UnitsQueueService>().AsSingle();
            Container.Bind<BattleTurnsController>().AsSingle();
            Container.Bind<GameResultEvaluator>().AsSingle();
        }
    }
}