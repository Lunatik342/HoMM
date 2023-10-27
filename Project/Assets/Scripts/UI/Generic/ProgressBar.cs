using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private Image _filledImage;

    public void SetFillAmount(float fillPercent)
    {
        _filledImage.fillAmount = fillPercent;
    }
}
