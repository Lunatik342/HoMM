using System.Collections.Generic;
using UnityEngine;

namespace UISystem
{
    [CreateAssetMenu(fileName = "UIWindowsCollection", menuName = "UIWindows/WindowsCollection")]
    public class UIWindowsCollection: ScriptableObject
    {
        [SerializeField] private List<UIWindow> _windows;

        public List<UIWindow> Windows => _windows;
    }
}