using System;
using System.Collections.Generic;
using FGJ24.Inventory;
using FGJ24.ScriptableObjects.UICrafting;
using Ioni;
using UnityEngine;

namespace FGJ24.CraftingStation
{
    public class CraftingStation : MonoBehaviour
    {
        [SerializeField] private List<Resource> stashedResources;
        private Inventory.Inventory _inventory;
        
        private void Awake()
        {
            _inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<Inventory.Inventory>();
        }

        public void StashResources()
        {
            stashedResources.AddRange(_inventory.Contents);
        }

        public void StartInteractWithCraftStation()
        {
            StashResources();
            _inventory.EmptyInventory();
        }

        public bool HasResourcesFor(RecipeBlueprint recipe)
        {
            var crystalCount = stashedResources.FindAll(r => r.ResourceType == ResourceType.Crystal).Count;
            var stoneCount = stashedResources.FindAll(r => r.ResourceType == ResourceType.Stone).Count;
            var mushroomCount = stashedResources.FindAll(r => r.ResourceType == ResourceType.Mushroom).Count;

            if (recipe.Crystals <= crystalCount && recipe.Stones <= stoneCount && recipe.Mushrooms <= mushroomCount)
            {
                return true;
            }

            return false;
        }

        public void RemoveResources(List<Resource> resources)
        {
            D.Info("Removing Resources: ", resources.Count);
            resources.ForEach(r =>
            {
                var resourceToRemove = stashedResources.Find(res => res.ResourceType == r.ResourceType);
                stashedResources.Remove(resourceToRemove);
            });
        }
    }
}
