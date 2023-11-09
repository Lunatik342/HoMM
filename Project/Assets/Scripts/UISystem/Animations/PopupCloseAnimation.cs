using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace UISystem.Animations
{
    public class PopupCloseAnimation: WindowAnimation
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Transform _transformForAnimation;
        [SerializeField] private float _closeAnimationDuration = 0.25f;
        [SerializeField] private float _startScale = 0.7f;

        public override async UniTask Animate(CancellationToken cancellationToken)
        {
            var  animationSequence = DOTween.Sequence();
            
            animationSequence.Join(_canvasGroup.DOFade(0, _closeAnimationDuration));
            animationSequence.Join(_transformForAnimation.DOScale(_startScale, _closeAnimationDuration));
            
            await animationSequence.ToUniTask(cancellationToken: cancellationToken).SuppressCancellationThrow();
        }
    }
}