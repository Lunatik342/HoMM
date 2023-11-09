using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UISystem.Animations
{
    public class LoadingScreenOpenAnimation: WindowAnimation
    {
        [SerializeField] private Image _fadeImage;
        [SerializeField] private float _contentFadeInDuration = 0.25f;

        public override async UniTask Animate(CancellationToken cancellationToken)
        {
            var sequence = DOTween.Sequence();
            
            sequence.Append(_fadeImage.DOFade(0, _contentFadeInDuration).From(1).SetEase(Ease.InQuart));
            
            await sequence.ToUniTask(cancellationToken: cancellationToken);
        }
    }
}