using Cysharp.Threading.Tasks;
using UnityEngine;

namespace UISystem
{
    public interface IUIWindow
    {
        public GameObject GameObject { get; }
        public void Init();
        public UniTask Open();
        public UniTask Close();
        public void Clear();
    }
}