using System;
using FGJ24.Interactions;
using FGJ24.Inventory;
using FGJ24.ScriptableObjects.UICrafting;
using Ioni.Extensions;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FGJ24.UI
{
    public class CraftingLayoutElement : MonoBehaviour
    {
        [SerializeField] private TMP_Text recipeNameText;
        [SerializeField] private Image recipeIconComponent;
        [SerializeField] private HorizontalLayoutGroup resourceRequirements;
        [SerializeField] private GameObject recipeResourcePrefab;

        private RecipeBlueprint recipe;

        private PlayerInteractions _playerInteractions;

        [SerializeField] private Sprite crystalIcon;
        [SerializeField] private Sprite stoneIcon;
        [SerializeField] private Sprite mushroomIcon;
        
        private void Start()
        {
            _playerInteractions = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInteractions>();
        }

        public void OnButtonClick()
        {
            "B".Info();
            _playerInteractions.TryCraftRecipe(recipe);
        }
        
        private Sprite IconFor(ResourceType resourceType)
        {
            switch (resourceType)
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
        
        public void SetContents(RecipeBlueprint recipe)
        {
            this.recipe = recipe;
            
            recipeNameText.text = recipe.RecipeName;
            recipeIconComponent.sprite = recipe.RecipeIcon;

            var recipeResource = Instantiate(recipeResourcePrefab, resourceRequirements.transform)
                .GetComponent<RecipeResource>();
            if (recipeResource != null)
            {
                recipeResource.SetContents("Crystals", recipe.Crystals, IconFor(ResourceType.Crystal));
            }
            
            var recipeResource2 = Instantiate(recipeResourcePrefab, resourceRequirements.transform)
                .GetComponent<RecipeResource>();
            if (recipeResource2 != null)
            {
                recipeResource2.SetContents("Stones", recipe.Stones, IconFor(ResourceType.Stone));
            }
            
            var recipeResource3 = Instantiate(recipeResourcePrefab, resourceRequirements.transform)
                .GetComponent<RecipeResource>();
            if (recipeResource3 != null)
            {
                recipeResource3.SetContents("Mushrooms", recipe.Mushrooms, IconFor(ResourceType.Mushroom));
            }
        }
    }
}
