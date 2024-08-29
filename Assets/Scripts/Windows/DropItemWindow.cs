using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Windows
{
    public class DropItemWindow : Window<DropItemWindowSetup>
    {
        [SerializeField] private TMP_Text _levelText;
        [SerializeField] private Image _itemImage;
        [SerializeField] private Button _sellButton;
        [SerializeField] private Button _takeButton;

        public event Action TakeEvent = delegate { };
        public event Action SellEvent = delegate { };

        private void OnEnable()
        {
            _sellButton.onClick.AddListener(OnSellButtonClicked);
            _takeButton.onClick.AddListener(OnTakeButtonClicked);
        }

        private void OnDisable()
        {
            _sellButton.onClick.RemoveListener(OnSellButtonClicked);
            _takeButton.onClick.RemoveListener(OnTakeButtonClicked);
        }

        public override void Init(DropItemWindowSetup windowSetup)
        {
            gameObject.SetActive(true);

            _itemImage.sprite = windowSetup.ItemSprite;
            _levelText.text = windowSetup.ItemLevel.ToString();
        }

        private void OnTakeButtonClicked()
        {
            TakeEvent();
            gameObject.SetActive(false);
        }

        private void OnSellButtonClicked()
        {
            SellEvent();
            gameObject.SetActive(false);
        }
    }
}
