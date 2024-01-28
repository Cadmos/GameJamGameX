using System.Collections.Generic;
using FGJ24.UI;
using UnityEngine;

namespace FGJ24.Inventory
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] private List<Resource> resources;
        [SerializeField] private UIResources UIResources;

        public void RemoveResource(ResourceType resourceType)
        {
            var toBeRemoved = resources.Find(r => r.ResourceType == resourceType);
            resources.Remove(toBeRemoved);
            UpdateUI();
        }

        public void AddResource(ResourceType resourceType)
        {
            resources.Add(new Resource(resourceType));
            UpdateUI();
        }

        private void UpdateUI()
        {
            UIResources.SetResources(resources);
        }

        public void EmptyInventory()
        {
            resources = new List<Resource>();
            UpdateUI();
        }

        public List<Resource> Contents => resources;
    }
}
