using System;
using System.Collections.Generic;
using Enums;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(menuName = "MadPixel/Configs/SlotItemSettingsData")]
    public class SlotItemSettingsData : ScriptableObject
    {
        [Serializable]
        public class SlotItemSettings
        {
            [SerializeField] private Slots _slot;
            [SerializeField] private int _level;
            [SerializeField] private int _sellGold;
            [SerializeField] private Sprite _itemSprite;

            public Slots Slot => _slot;
            public int Level => _level;
            public int SellGold => _sellGold;
            public Sprite ItemSprite => _itemSprite;
        }

        [SerializeField] private List<SlotItemSettings> _slotItemSettings = new();

        public IReadOnlyList<SlotItemSettings> SlotItemSettingsList => _slotItemSettings;
    }
}
