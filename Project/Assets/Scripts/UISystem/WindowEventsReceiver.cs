using UnityEngine;

namespace UISystem
{
    public abstract class WindowEventsReceiver: MonoBehaviour
    {
        public virtual void OnInit() { }
        public virtual void OnShow() { }
        public virtual void OnHide() { }
        public virtual void OnClear() { }
    }
}