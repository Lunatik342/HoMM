using Battle.Units.Components;
using TMPro;
using UnityEngine;

namespace UI.AttackInfoDisplayer
{
    public class AttackInfoDisplayer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _damageInfoText;
        [SerializeField] private TextMeshProUGUI _retaliationInfoText;
        [SerializeField] private Canvas _parentCanvas;
        [SerializeField] private RectTransform _parentCanvasRectTransform;
        
        public void ShowInfo(MinMaxValue casualtiesPrediction, bool willRetaliate)
        {
            gameObject.SetActive(true);

            var killCountString = casualtiesPrediction is { Min: 0, Max: 0 }
                ? "0" : $"{casualtiesPrediction.Min} - {casualtiesPrediction.Max}";
            
            _damageInfoText.text = $"Kills: {killCountString}";
            _retaliationInfoText.text = $"Retaliate: {(willRetaliate ? "Yes" : "No")}";
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private void LateUpdate()
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_parentCanvasRectTransform, 
                Input.mousePosition, 
                _parentCanvas.worldCamera,
                out var localPoint);

            transform.position = _parentCanvas.transform.TransformPoint(localPoint);
        }
    }
}
