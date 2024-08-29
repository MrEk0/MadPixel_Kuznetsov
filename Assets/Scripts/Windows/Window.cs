using UnityEngine;

namespace Windows
{
    public abstract class Window<T> : MonoBehaviour where T : WindowSetup, new()
    {
        public abstract void Init(T windowSetup);
    }
}
