using UISystem;
using UnityEngine;
using UnityEngine.UI;

namespace UI.LoadingScreen
{
    public class LoadingWindow : UIWindow
    {
        [SerializeField] private Sprite[] _backgroundSprites;
        [SerializeField] private Image _backgroundImage;

        public override void OnShow()
        {
            _backgroundImage.sprite = _backgroundSprites[Random.Range(0, _backgroundSprites.Length)];
        }
    }
}
