using System.Linq;
using System.Threading;
using Battle;
using Cysharp.Threading.Tasks;
using Infrastructure.GlobalStateMachine;
using UI.GenericUIComponents;
using UI.LoadingScreen;
using UISystem;
using UnityEngine;
using UnityEngine.UI;
using Utilities;
using Zenject;

namespace UI.MainMenu
{
    public class MainMenu : UIWindow
    {
        [SerializeField] private AnimatedSpriteSwapper _animatedSpriteSwapper;
        [SerializeField] private Sprite[] _backgroundSprites;
        [SerializeField] private int _delaysBetweenSpriteSwapInMilliSecs;
        [SerializeField] private Button _startGameFun;
        [SerializeField] private Button _startGameNormal;
        [SerializeField] private Button _startGameHard;

        private GameStateMachine _gameStateMachine;
        private UIWindowsManager _uiWindowsManager;
        private BattleStartParametersProvider _battleStartParametersProvider;
        
        private CancellationTokenSource _cancellationTokenSource;

        [Inject]
        public void Construct(GameStateMachine gameStateMachine, UIWindowsManager uiWindowsManager, BattleStartParametersProvider battleStartParametersProvider)
        {
            _gameStateMachine = gameStateMachine;
            _uiWindowsManager = uiWindowsManager;
            _battleStartParametersProvider = battleStartParametersProvider;
        }

        public override void OnInit()
        {
            _startGameNormal.onClick.AddListener(() => LoadBattleLevel(1, 1f));
            _startGameHard.onClick.AddListener(() => LoadBattleLevel(4, 1.1f));
            _startGameFun.onClick.AddListener(LoadFunLevel);
        }

        private async void LoadBattleLevel(int weeksCount, float difficultyModifier)
        {
            await _uiWindowsManager.OpenWindow<LoadingWindow>();
            _gameStateMachine.Enter<BattleState, BattleStartParameters>(_battleStartParametersProvider.GetDefault(weeksCount, difficultyModifier));
        }
        
        private async void LoadFunLevel()
        {
            await _uiWindowsManager.OpenWindow<LoadingWindow>();
            _gameStateMachine.Enter<BattleState, BattleStartParameters>(_battleStartParametersProvider.GetEasyFunBattleParameters());
        }

        public override void OnShow()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            StartBackgroundSwapSequence(_cancellationTokenSource.Token).SuppressCancellationThrow().Forget(Debug.LogError);
        }

        public override void OnHide()
        {
            EndBackgroundSwapSequence();
        }

        private async UniTask StartBackgroundSwapSequence(CancellationToken cancellationToken)
        {
            var shuffledSprites = _backgroundSprites.Shuffle().ToList();
            _animatedSpriteSwapper.SwapTo(shuffledSprites[^1]);

            while (true)
            {
                foreach (var sprite in shuffledSprites)
                {
                    await UniTask.Delay(_delaysBetweenSpriteSwapInMilliSecs, DelayType.Realtime, PlayerLoopTiming.Update, cancellationToken);
                    await _animatedSpriteSwapper.AnimatedSwapTo(sprite, cancellationToken);
                }
            }
        }

        private void EndBackgroundSwapSequence()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
        }
    }
}
