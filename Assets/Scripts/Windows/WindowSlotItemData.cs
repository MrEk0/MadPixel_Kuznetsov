using System;
using UnityEngine;

namespace Windows
{
    [Serializable]
    public class WindowSlotItemData
    {
        public bool IsTaken = false;
        public Sprite ItemSprite = null;
        public int Level = 0;
    }
}
