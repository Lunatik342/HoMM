using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Utilities;

namespace UISystem
{
    public class UIWindow: WindowEventsReceiver, IUIWindow
    {
        [SerializeField] private WindowEventsReceiver[] _eventsReceivers;
        [SerializeField] private WindowAnimation _openAnimation;
        [SerializeField] private WindowAnimation _closeAnimation;

        private CancellationTokenSource _animationTokenSource;

        GameObject IUIWindow.GameObject => gameObject;

        void IUIWindow.Init()
        {
            OnInit();
            _eventsReceivers.Foreach(r => r.OnInit());
        }
        
        async UniTask IUIWindow.Open()
        {
            OnShow();
            _eventsReceivers.Foreach(r => r.OnShow());
            gameObject.SetActive(true);

            var token = HandleAnimationCancellation();

            if (_openAnimation != null)
            {
                await _openAnimation.Animate(token);
            }
        }

        async UniTask IUIWindow.Close()
        {
            var token = HandleAnimationCancellation();

            if (_closeAnimation != null)
            {
                await _closeAnimation.Animate(token);
            }
            
            gameObject.SetActive(false);
            OnHide();
            _eventsReceivers.Foreach(r => r.OnHide());
        }

        void IUIWindow.Clear()
        {
            OnClear();
            _eventsReceivers.Foreach(r => r.OnClear());
        }

        private CancellationToken HandleAnimationCancellation()
        {
            StopCurrentAnimation();
            _animationTokenSource = new CancellationTokenSource();
            return _animationTokenSource.Token;
        }

        private void StopCurrentAnimation()
        {
            _animationTokenSource?.Cancel();
            _animationTokenSource?.Dispose();
            _animationTokenSource = null;
        }
    }
}