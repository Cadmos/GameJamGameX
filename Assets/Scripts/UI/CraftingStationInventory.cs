using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FGJ24.UI
{
    public class CraftingStationInventory : MonoBehaviour
    {
        [SerializeField] private TMP_Text quantityCrystals;
        [SerializeField] private TMP_Text quantityStones;
        [SerializeField] private TMP_Text quantityMushrooms;

        [SerializeField] private Image imageCrystals;
        [SerializeField] private Image imageStones;
        [SerializeField] private Image imageMushrooms;
        
        [SerializeField] private Sprite iconCrystals;
        [SerializeField] private Sprite iconStones;
        [SerializeField] private Sprite iconMushrooms;
        private void Start()
        {
            quantityCrystals.text = "0";
            quantityStones.text = "0";
            quantityMushrooms.text = "0";

            imageCrystals.sprite = iconCrystals;
            imageStones.sprite = iconStones;
            imageMushrooms.sprite = iconMushrooms;
        }

        public void SetQuantities(int crystals, int stones, int mushrooms)
        {
            quantityCrystals.text = crystals.ToString();
            quantityStones.text = stones.ToString();
            quantityMushrooms.text = mushrooms.ToString();
        }
    }
}
