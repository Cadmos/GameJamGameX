using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FGJ24
{
    public class RecipeResource : MonoBehaviour
    {
        // [SerializeField] private TMP_Text resourceNameComponent;  // TODO - NEEDED?
        [SerializeField] private TMP_Text resourceQuantityComponent; 
        [SerializeField] private Image resourceImageComponent;
        [SerializeField] private string resourceName; 

        public void SetContents(string name, int quantity, Sprite resourceIcon)
        {
            resourceQuantityComponent.text = quantity.ToString();
            resourceImageComponent.sprite = resourceIcon;
            resourceName = name;
        }
    }
}
