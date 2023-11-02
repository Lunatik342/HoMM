using UnityEngine;
using UnityEngine.UI;

namespace UI.GenericUIComponents
{
    public class ProgressBar : MonoBehaviour
    {
        [SerializeField] private Image _filledImage;

        public void SetFillAmount(float fillPercent)
        {
            _filledImage.fillAmount = fillPercent;
        }
    }
}
