using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

namespace UI.GenericUIComponents
{
    public class AnimatedSpriteSwapper : MonoBehaviour
    {
        [SerializeField] private Image _bottomLayerImage;
        [SerializeField] private Image _topLayerImage;
        [SerializeField] private float _swapDuration;

        private Tween _swapTween;

        public void SwapTo(Sprite sprite)
        {
            StopAnimation();
            
            _topLayerImage.color = ColorUtilities.TransparentWhite;
            _bottomLayerImage.color = Color.white;
            _bottomLayerImage.sprite = sprite;
            _topLayerImage.sprite = sprite;
        }
        
        public async UniTask AnimatedSwapTo(Sprite sprite, CancellationToken token = default)
        {
            StopAnimation();
            
            _bottomLayerImage.sprite = _topLayerImage.sprite;
            _bottomLayerImage.color = Color.white;
            
            _topLayerImage.sprite = sprite;
            _topLayerImage.color = ColorUtilities.TransparentWhite;

            _swapTween = _topLayerImage.DOFade(1, _swapDuration);
            await _swapTween.WithCancellation(token);
        }

        private void StopAnimation()
        {
            _swapTween?.Kill();
            _swapTween = null;
        }
    }
}
