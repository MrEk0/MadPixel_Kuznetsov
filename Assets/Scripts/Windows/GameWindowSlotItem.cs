using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Windows
{
    public class GameWindowSlotItem : MonoBehaviour
    {
        [SerializeField] private RectTransform _slotTransform;
        [SerializeField] private Image _slotImage;
        [SerializeField] private TMP_Text _levelText;

        public void UpdateView(WindowSlotItemData windowSlotData)
        {
            _slotTransform.gameObject.SetActive(windowSlotData.IsTaken);
            _slotImage.sprite = windowSlotData.ItemSprite;
            _levelText.text = windowSlotData.Level.ToString();
        }
    }
}
