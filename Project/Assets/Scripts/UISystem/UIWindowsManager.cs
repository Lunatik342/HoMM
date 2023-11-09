using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace UISystem
{
    public class UIWindowsManager: MonoBehaviour
    {
        private UIWindowsFactory _windowsFactory;

        private List<IUIWindow> _openWindows = new();

        [Inject]
        public void Construct(UIWindowsFactory windowsFactory)
        {
            _windowsFactory = windowsFactory;
        }

        public async UniTask<T> OpenWindow<T>() where T : IUIWindow
        {
            var window = _windowsFactory.GetWindow<T>();
            window.GameObject.transform.SetParent(transform, false);
            window.Init();
            _openWindows.Add(window);
            await window.Open();
            return window;
        }

        public async UniTask CloseWindow<T>() where T : IUIWindow
        {
            var window = _openWindows.First(w => w.GetType() == typeof(T));
            await CloseWindow(window);
        }

        public async UniTask CloseWindow(IUIWindow window)
        {
            _openWindows.Remove(window);
            await window.Close();
            window.Clear();
            Destroy(window.GameObject);
        }
    }
}