using System;
using Battle;
using Cysharp.Threading.Tasks;
using UI.LoadingScreen;
using UnityEngine.SceneManagement;
using Zenject;

namespace Infrastructure
{
    public class SceneLoader
    {
        private readonly ZenjectSceneLoader _sceneLoader;
        private readonly LoadingScreen _loadingScreen;

        private const int _minimumLoadingScreenShownDuration = 750;

        public SceneLoader(ZenjectSceneLoader sceneLoader, LoadingScreen loadingScreen)
        {
            _sceneLoader = sceneLoader;
            _loadingScreen = loadingScreen;
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
            await _loadingScreen.Show();
            await UniTask.WhenAll(
                _sceneLoader.LoadSceneAsync(sceneName, loadSceneMode, extraBindings).ToUniTask(),
                UniTask.Delay(_minimumLoadingScreenShownDuration));
            await _loadingScreen.Hide();
        }
    }
}