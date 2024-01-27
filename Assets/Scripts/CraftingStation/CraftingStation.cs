using System;
using System.Collections.Generic;
using FGJ24.Inventory;
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

        public void StashResource(Resource resource)
        {
            stashedResources.Add(resource);
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
    }
}
