using FGJ24.Inventory;
using TMPro;
using UnityEngine;

namespace FGJ24.UI
{
    public class UIResource : MonoBehaviour
    {
        [SerializeField] private ResourceType resourceType;
        private int _quantity;

        [SerializeField] private TMP_Text quantityText; 

        public void UpdateContent(ResourceType resourceType, int quantity)
        {
            _quantity = quantity;
            this.resourceType = resourceType;

            quantityText.text = _quantity.ToString();
        }
    }
}
