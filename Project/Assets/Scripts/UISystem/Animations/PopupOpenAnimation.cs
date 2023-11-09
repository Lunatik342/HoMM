using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace UISystem.Animations
{
    public class PopupOpenAnimation: WindowAnimation
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Transform _transformForAnimation;
        [SerializeField] private float _openAnimationDuration = 0.25f;
        [SerializeField] private float _startScale = 0.7f;
        public override async UniTask Animate(CancellationToken cancellationToken)
        {
            var animationSequence = DOTween.Sequence();
            
            animationSequence.Join(_canvasGroup.DOFade(1, _openAnimationDuration).From(0));
            animationSequence.Join(_transformForAnimation.DOScale(1, _openAnimationDuration).From(_startScale));
            
            await animationSequence.ToUniTask(cancellationToken: cancellationToken).SuppressCancellationThrow();
        }
    }
}