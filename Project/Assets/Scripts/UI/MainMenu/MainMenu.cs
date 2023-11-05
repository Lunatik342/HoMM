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
using UI.GenericUIComponents;
using UnityEngine;
using UnityEngine.UI;
using Utilities;
using Zenject;
using Random = UnityEngine.Random;

namespace UI.MainMenu
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private AnimatedSpriteSwapper _animatedSpriteSwapper;
        [SerializeField] private Sprite[] _backgroundSprites;
        [SerializeField] private int _delaysBetweenSpriteSwapInMilliSecs;
        [SerializeField] private Button _startGameButton;

        private CancellationTokenSource _cancellationTokenSource;

        private SceneLoader _sceneLoader;

        [Inject]
        public void Construct(SceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }
        
        private void Start()
        {
            _startGameButton.onClick.AddListener(LoadBattleLevel);
            Show();
        }

        private void OnDestroy()
        {
            Hide();
        }

        private void LoadBattleLevel()
        {
            _sceneLoader.LoadBattleScene(CreateBattleStartParametersTEMPORARY()).Forget(Debug.LogError);
        }

        public void Show()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            StartBackgroundSwapSequence(_cancellationTokenSource.Token).SuppressCancellationThrow().Forget(Debug.LogError);
        }

        public void Hide()
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
                        new UnitCreationParameter(new Vector2Int(0, 0), UnitId.Blank, 5),
                        new UnitCreationParameter(new Vector2Int(1, 4), UnitId.Blank, 15),
                        new UnitCreationParameter(new Vector2Int(1, 9), UnitId.Blank, 10),
                    }
                },
                {
                    Team.TeamRight, new List<UnitCreationParameter>
                    {
                        new UnitCreationParameter(new Vector2Int(10, 2), UnitId.Blank, 33),
                        new UnitCreationParameter(new Vector2Int(11, 6), UnitId.Blank, 62),
                        new UnitCreationParameter(new Vector2Int(10, 8), UnitId.Blank, 12),
                    }
                },
            };

            var testBattleStartParameters = new BattleStartParameters(BattleArenaId.Blank, new ObstacleGenerationParameters()
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
                { Team.TeamRight, CommandProviderType.PlayerControlled },
            });
            return testBattleStartParameters;
        }
    }
}
