using System;
using System.Collections.Generic;
using Enums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Windows
{
    public class GameWindow : Window<GameWindowSetup>
    {
        [Serializable]
        public class WindowSlotItem
        {
            public Slots Slot;
            public GameWindowSlotItem SlotItem;
        }

        [SerializeField] private TMP_Text _goldCountText;
        [SerializeField] private TMP_Text _manaCountText;
        [SerializeField] private Slider _manaSlider;
        [SerializeField] private Button _playButton;
        [SerializeField] private List<WindowSlotItem> _windowSlotItems = new();

        public event Action PlayEvent = delegate { };

        private void OnEnable()
        {
            _playButton.onClick.AddListener(OnPlayButtonClicked);
        }

        private void OnDisable()
        {
            _playButton.onClick.RemoveListener(OnPlayButtonClicked);
        }

        public override void Init(GameWindowSetup windowSetup)
        {
            _goldCountText.text = windowSetup.GoldCount.ToString();
            _manaCountText.text = windowSetup.ManaCount.ToString();
            _manaSlider.minValue = 0;
            _manaSlider.maxValue = windowSetup.MaxSliderValue;
            _manaSlider.value = windowSetup.ManaCount;

            foreach (var slotItem in _windowSlotItems)
            {
                if (windowSetup.SlotsDictionary.TryGetValue(slotItem.Slot, out var data))
                {
                    slotItem.SlotItem.UpdateView(data);
                    continue;
                }

                slotItem.SlotItem.UpdateView(new WindowSlotItemData());
            }
        }

        public void UpdateSlider(int newValue)
        {
            _manaSlider.value = newValue;
            _manaCountText.text = newValue.ToString();
        }

        private void OnPlayButtonClicked()
        {
            PlayEvent();
        }
    }
}
