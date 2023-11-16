using UI.AttackInfoDisplayer;
using UnityEngine;
using Zenject;

namespace Battle
{
    public class UIInstaller : MonoInstaller
    {
        [SerializeField] private AttackInfoDisplayer _attackInfoDisplayer;
        
        public override void InstallBindings()
        {
            Container.BindInstance(_attackInfoDisplayer);
        }
    }
}
