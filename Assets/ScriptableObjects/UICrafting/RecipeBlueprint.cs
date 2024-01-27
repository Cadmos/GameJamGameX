using System.Collections.Generic;
using FGJ24.Interactions;
using FGJ24.Inventory;
using UnityEngine;

namespace FGJ24.ScriptableObjects.UICrafting
{
    
    [CreateAssetMenu(fileName = "RecipeBlueprint", menuName = "Crafting/RecipeBlueprint", order = 1)]
    public class RecipeBlueprint : ScriptableObject
    {
        [SerializeField] private int id;
        [SerializeField] private string recipeName;
        [SerializeField] private Sprite recipeIcon;
        
        [SerializeField] private int crystals;
        [SerializeField] private int stones;
        [SerializeField] private int mushrooms;

        public int Id => id;
        public string RecipeName => recipeName;
        public Sprite RecipeIcon => recipeIcon;
        public int Crystals => crystals;
        public int Stones => stones;
        public int Mushrooms => mushrooms;
    }
}
