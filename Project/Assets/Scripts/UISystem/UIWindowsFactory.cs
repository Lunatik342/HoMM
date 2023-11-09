using System;
using System.Collections.Generic;
using Zenject;

namespace UISystem
{
    public class UIWindowsFactory
    {
        private readonly IInstantiator _instantiator;
        private readonly UIWindowsCollection _uiWindowsCollection;

        private Dictionary<Type, IUIWindow> _windowsByType;

        public UIWindowsFactory(IInstantiator instantiator, UIWindowsCollection uiWindowsCollection)
        {
            _instantiator = instantiator;
            _uiWindowsCollection = uiWindowsCollection;
        }
        
        public void Setup()
        {
            _windowsByType = new Dictionary<Type, IUIWindow>();
            
            foreach (var window in _uiWindowsCollection.Windows)
            {
                var type = window.GetType();
                _windowsByType.Add(type, window);
            }
        }

        public T GetWindow<T>() where T: IUIWindow
        {
            var prefab = GetPrefabForWindow<T>();
            var result = _instantiator.InstantiatePrefabForComponent<T>(prefab.GameObject);
            return result;
        }

        private T GetPrefabForWindow<T>() where T: IUIWindow
        {
            return (T) _windowsByType[typeof(T)];
        }
    }
}