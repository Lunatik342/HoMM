using Battle.BattleFlow;
using Cysharp.Threading.Tasks;
using TMPro;
using UISystem;
using UnityEngine;
using UnityEngine.UI;

namespace UI.BattleResultWindow
{
    public class BattleResultWindow : UIWindow
    {
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _mainMenuButton;
        [SerializeField] private TextMeshProUGUI _resultText;

        private BattleResultData _battleResultData;
        private UniTaskCompletionSource<BattleResultWindowOutput> _outputCompletionSource;
        
        public override void OnInit()
        {
            _restartButton.onClick.AddListener(RestartBattle);
            _mainMenuButton.onClick.AddListener(GoToMainMenu);
        }

        public void PassParameters(BattleResultData battleResultData, UniTaskCompletionSource<BattleResultWindowOutput> outputCompletionSource)
        {
            _battleResultData = battleResultData;
            _outputCompletionSource = outputCompletionSource;
        }

        public override void OnShow()
        {
            var teamName = _battleResultData.WonTeam.ToString();
            _resultText.text = $"{teamName} won";
        }

        private void RestartBattle()
        {
            _outputCompletionSource.TrySetResult(BattleResultWindowOutput.RestartBattle);
        }

        private void GoToMainMenu()
        {
            _outputCompletionSource.TrySetResult(BattleResultWindowOutput.ReturnToMainMenu);
        }
    }
}
