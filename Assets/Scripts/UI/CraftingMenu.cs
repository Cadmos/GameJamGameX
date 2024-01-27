using System.Collections;
using System.Collections.Generic;
using FGJ24.ScriptableObjects.UICrafting;
using FGJ24.UI;
using Ioni.Extensions;
using UnityEngine;

namespace FGJ24
{
    public class CraftingMenu : MonoBehaviour
    {
        [SerializeField] private CraftingLayoutGroup layoutGroup;
        [SerializeField] private List<RecipeBlueprint> recipes;

        public void ShowCraftingMenu()
        {
            "Showing Crafting Menu".Info();
            layoutGroup.UpdateUI(recipes);
        }
    }
}
