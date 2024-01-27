using System;
using FGJ24.Inventory;
using Ioni.Extensions;
using UnityEngine;

namespace FGJ24.Interactions
{
    public class Gatherable : Interactable
    {
        [SerializeField] private ResourceType resourceType;

        [SerializeField] private int ResourceQuantity;
        public ResourceType ResourceType => resourceType;

        private Inventory.Inventory _inventory;

        private void Start()
        {
            _inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<Inventory.Inventory>();
        }

        public override void Interact()
        {
            if (ResourceQuantity <= 0) return;
            _inventory.AddResource(resourceType);
            ResourceQuantity -= 1;
        }
    }
}
