using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Infrastructure;
using Infrastructure.SimpleStateMachine;
using UI.BattleResultWindow;
using UI.Hud;
using UI.LoadingScreen;
using UISystem;
using UnityEngine;

namespace Battle.BattleFlow.Phases
{
    public class BattleEndPhase: IState
    {
        private readonly GameResultEvaluator _gameResultEvaluator;
        private readonly BattleStartParameters _battleStartParameters;
        private readonly UIWindowsManager _uiWindowsManager;
        private readonly GameStateMachine _gameStateMachine;

        public BattleEndPhase(GameResultEvaluator gameResultEvaluator, 
            BattleStartParameters battleStartParameters,
            UIWindowsManager uiWindowsManager,
            GameStateMachine gameStateMachine)
        {
            _gameResultEvaluator = gameResultEvaluator;
            _battleStartParameters = battleStartParameters;
            _uiWindowsManager = uiWindowsManager;
            _gameStateMachine = gameStateMachine;
        }
        
        public void Enter()
        {
            HandleBattleEnd().Forget(Debug.LogError);
        }

        private async UniTask HandleBattleEnd()
        {
            var battleResultData = _gameResultEvaluator.CalculateBattleResultData(_battleStartParameters);
            var nextStep = await ShowBattleResultScreen(battleResultData);
            await LoadNextGameState(nextStep);
        }

        private async Task<BattleResultWindowOutput> ShowBattleResultScreen(BattleResultData battleResultData)
        {
            _uiWindowsManager.CloseWindow<UnitsQueueWindow>().Forget(Debug.LogError);
            var resultWindowCompletionSource = new UniTaskCompletionSource<BattleResultWindowOutput>();
            
            await _uiWindowsManager.OpenWindow<BattleResultWindow>(window =>
            {
                window.PassParameters(battleResultData, resultWindowCompletionSource);
            });

            var nextStep = await resultWindowCompletionSource.Task;
            await _uiWindowsManager.CloseWindow<BattleResultWindow>();
            return nextStep;
        }

        private async Task LoadNextGameState(BattleResultWindowOutput nextStep)
        {
            await _uiWindowsManager.OpenWindow<LoadingWindow>();

            switch (nextStep)
            {
                case BattleResultWindowOutput.RestartBattle:
                    _gameStateMachine.Enter<BattleState, BattleStartParameters>(_battleStartParameters);
                    break;
                case BattleResultWindowOutput.ReturnToMainMenu:
                    _gameStateMachine.Enter<MainMenuState>();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Exit()
        {
            
        }
    }
}