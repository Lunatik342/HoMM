using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UISystem.Animations
{
    public class LoadingScreenCloseAnimation: WindowAnimation
    {
        [SerializeField] private Image _fadeImage;
        [SerializeField] private float _contentFadeOutDuration = 0.25f;
        
        public override async UniTask Animate(CancellationToken cancellationToken)
        {
            var sequence = DOTween.Sequence();

            sequence.Append(_fadeImage.DOFade(1f, _contentFadeOutDuration).SetEase(Ease.OutQuart));
            
            await sequence.ToUniTask(cancellationToken: cancellationToken);
        }
    }
}