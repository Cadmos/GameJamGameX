using FGJ24.Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FGJ24.UI
{
    public class UIResource : MonoBehaviour
    {
        [SerializeField] private ResourceType resourceType;
        private int _quantity;

        [SerializeField] private TMP_Text quantityText;
        [SerializeField] private Image resourceImageComponent;
        
        [SerializeField] private Sprite crystalIcon;
        [SerializeField] private Sprite stoneIcon;
        [SerializeField] private Sprite mushroomIcon;

        public void UpdateContent(ResourceType resourceType, int quantity)
        {
            _quantity = quantity;
            this.resourceType = resourceType;

            resourceImageComponent.sprite = IconFor(resourceType);
            quantityText.text = _quantity.ToString();
        }

        private Sprite IconFor(ResourceType resourceType)
        {
            switch (this.resourceType)
            {
                case ResourceType.Crystal:
                    return crystalIcon;
                case ResourceType.Stone:
                    return stoneIcon;
                case ResourceType.Mushroom:
                    return mushroomIcon;
                default:
                    return crystalIcon;
            }
        }
    }
}
