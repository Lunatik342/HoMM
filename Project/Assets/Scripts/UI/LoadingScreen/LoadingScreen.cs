using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

namespace UI.LoadingScreen
{
    public class LoadingScreen : MonoBehaviour
    {
        [SerializeField] private Sprite[] _backgroundSprites;
        [SerializeField] private Image _backgroundImage;
        [SerializeField] private GameObject _content;
        [SerializeField] private Image _fadeImage;
        [SerializeField] private CanvasGroup _canvasGroup;

        [SerializeField] private float _contentFadeInDuration = 0.25f;
        [SerializeField] private float _contentFadeOutDuration = 0.25f;
        [SerializeField] private float _screenFadeDuration = 0.15f;

        public async UniTask Show()
        {
            SetStartingState();
            await AnimateAppear();
        }

        public async UniTask Hide()
        {
            await AnimateHide();
        }

        private void SetStartingState()
        {
            gameObject.SetActive(true);
            _fadeImage.SetAlpha(1);
            _canvasGroup.alpha = 0;
            _content.gameObject.SetActive(false);

            _backgroundImage.sprite = _backgroundSprites[Random.Range(0, _backgroundSprites.Length)];
        }

        private async Task AnimateAppear()
        {
            await _canvasGroup.DOFade(1, _screenFadeDuration).ToUniTask();
            _content.gameObject.SetActive(true);
            await _fadeImage.DOFade(0, _contentFadeInDuration).From(1).SetEase(Ease.InQuart).ToUniTask();
        }

        private async Task AnimateHide()
        {
            await _fadeImage.DOFade(1f, _contentFadeOutDuration).SetEase(Ease.OutQuart).ToUniTask();
            _content.gameObject.SetActive(false);
            await _canvasGroup.DOFade(0, _screenFadeDuration).ToUniTask();
            gameObject.SetActive(false);
        }
    }
}
