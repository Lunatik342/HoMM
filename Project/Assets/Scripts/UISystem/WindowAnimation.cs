using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace UISystem
{
    public abstract class WindowAnimation: MonoBehaviour
    {
        public abstract UniTask Animate(CancellationToken cancellationToken);
    }
}