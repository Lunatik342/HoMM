using System;
using Battle;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using Zenject;

namespace Infrastructure
{
    public class SceneLoader
    {
        private readonly ZenjectSceneLoader _sceneLoader;

        public SceneLoader(ZenjectSceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }

        public async UniTask LoadBattleScene(BattleStartParameters battleStartParameters)
        {
            await LoadScene(SceneNames.BattleSceneName, LoadSceneMode.Single, container =>
            {
                container.BindInstance(battleStartParameters).WhenInjectedInto<BattleBootstrapper>();
            });
        }
        
        public async UniTask LoadMainMenuScene()
        {
            await LoadScene(SceneNames.MainMenuSceneName, LoadSceneMode.Single);
        }
        
        private async UniTask LoadScene(string sceneName, LoadSceneMode loadSceneMode, Action<DiContainer> extraBindings = null)
        {
            await _sceneLoader.LoadSceneAsync(sceneName, loadSceneMode, extraBindings).ToUniTask();
        }
    }
}