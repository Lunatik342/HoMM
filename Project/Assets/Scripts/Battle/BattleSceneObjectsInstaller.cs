using UI.AttackInfoDisplayer;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Battle
{
    public class BattleSceneObjectsInstaller : MonoInstaller
    {
        [SerializeField] private AttackPredictionInfoDisplayer _attackPredictionInfoDisplayer;
        [SerializeField] private Camera _mainCamera;
        
        public override void InstallBindings()
        {
            Container.BindInstance(_attackPredictionInfoDisplayer).AsSingle();
            Container.BindInstance(_mainCamera).AsSingle();
        }
    }
}
