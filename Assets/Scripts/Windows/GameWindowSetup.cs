using System.Collections.Generic;
using Enums;

namespace Windows
{
    public class GameWindowSetup : WindowSetup
    {
        public int ManaCount;
        public int MaxSliderValue;
        public int GoldCount;
        public Dictionary<Slots, WindowSlotItemData> SlotsDictionary = new();
    }
}
