using DG.Tweening;
using TMPro;
using UnityEngine;

namespace UI.Generic
{
    public class AnimatedNumberChanger : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private float _animationDuration;

        private int _previousValue;
        private Tween _tween;

        public void SetValue(int value)
        {
            _text.text = value.ToString();
            _previousValue = value;
        }

        public void SetValueAnimated(int value)
        {
            _tween?.Kill();
        
            _tween = DOTween.To(() => _previousValue, x =>
            {
                _previousValue = x;
                _text.text = x.ToString();
            }, value, _animationDuration);
        }
    }
}
