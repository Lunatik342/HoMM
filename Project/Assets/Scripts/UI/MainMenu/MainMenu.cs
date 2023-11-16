using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Battle;
using Battle.Arena.Obstacles;
using Battle.Arena.StaticData;
using Battle.UnitCommands.Providers;
using Battle.Units.StaticData;
using Cysharp.Threading.Tasks;
using Infrastructure;
using Infrastructure.GlobalStateMachine;
using UI.GenericUIComponents;
using UI.LoadingScreen;
using UISystem;
using UnityEngine;
using UnityEngine.UI;
using Utilities;
using Zenject;
using Random = UnityEngine.Random;

namespace UI.MainMenu
{
    public class MainMenu : UIWindow
    {
        [SerializeField] private AnimatedSpriteSwapper _animatedSpriteSwapper;
        [SerializeField] private Sprite[] _backgroundSprites;
        [SerializeField] private int _delaysBetweenSpriteSwapInMilliSecs;
        [SerializeField] private Button _startGameButton;

        private GameStateMachine _gameStateMachine;
        private UIWindowsManager _uiWindowsManager;
        
        private CancellationTokenSource _cancellationTokenSource;

        [Inject]
        public void Construct(GameStateMachine gameStateMachine, UIWindowsManager uiWindowsManager)
        {
            _gameStateMachine = gameStateMachine;
            _uiWindowsManager = uiWindowsManager;
        }

        public override void OnInit()
        {
            _startGameButton.onClick.AddListener(LoadBattleLevel);
        }

        private async void LoadBattleLevel()
        {
            await _uiWindowsManager.OpenWindow<LoadingWindow>();
            _gameStateMachine.Enter<BattleState, BattleStartParameters>(CreateBattleStartParametersTEMPORARY());
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

        private static BattleStartParameters CreateBattleStartParametersTEMPORARY()
        {
            var unitsToSpawn = new Dictionary<Team, List<UnitCreationParameter>>()
            {
                {
                    Team.TeamLeft, new List<UnitCreationParameter>
                    {
                        new UnitCreationParameter(new Vector2Int(0, 0), UnitId.Peasant, 33),
                        new UnitCreationParameter(new Vector2Int(1, 4), UnitId.Archer, 50),
                        new UnitCreationParameter(new Vector2Int(1, 9), UnitId.King, 100),
                    }
                },
                {
                    Team.TeamRight, new List<UnitCreationParameter>
                    {
                        new UnitCreationParameter(new Vector2Int(10, 2), UnitId.Swordsman, 33),
                        new UnitCreationParameter(new Vector2Int(11, 6), UnitId.King, 50),
                        new UnitCreationParameter(new Vector2Int(10, 8), UnitId.Cavalier, 100),
                    }
                },
            };

            var testBattleStartParameters = new BattleStartParameters(BattleArenaId.Forge, new ObstacleGenerationParameters()
            {
                IsRandom = true,
                RandomSeed = Random.Range(0, Int32.MaxValue),
                DeterminedObstacleParameters = new List<ObstacleOnGridParameters>()
                {
                    new(ObstacleId.Blank1, ObstaclesSpawner.ObstacleRotationAngle.Degrees0, new Vector2Int(5, 5))
                },
            }, unitsToSpawn, new Dictionary<Team, CommandProviderType>
            {
                { Team.TeamLeft, CommandProviderType.PlayerControlled },
                { Team.TeamRight, CommandProviderType.AIControlled },
            });
            return testBattleStartParameters;
        }
    }
}
