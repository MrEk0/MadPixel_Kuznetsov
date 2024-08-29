using System;
using Windows;
using Enums;
using UnityEngine;

namespace Save
{
    [Serializable]
    public class SaveData
    {
        [SerializeField]
        SerializedDateTime _lastManaUpdate = new();
        
        [SerializeField]
        SerializedDictionary<Slots, WindowSlotItemData> _slotItems = new();
        
        public bool IsFirstEntrance;
        public int GoldCount;
        public int ManaCount;

        public SerializedDateTime LastManaUpdate => _lastManaUpdate;
        public SerializedDictionary<Slots, WindowSlotItemData> SlotItems => _slotItems;

        public void ChangeSlot(Slots slot, WindowSlotItemData data)
        {
            if (SlotItems.ContainsKey(slot))
            {
                SlotItems[slot].Level = data.Level;
                SlotItems[slot].ItemSprite = data.ItemSprite;
                SlotItems[slot].IsTaken = data.IsTaken;
            }
            else
            {
                SlotItems.Add(slot, new WindowSlotItemData
                {
                    Level = data.Level,
                    ItemSprite = data.ItemSprite,
                    IsTaken = data.IsTaken
                });
            }
        }
    }
}
