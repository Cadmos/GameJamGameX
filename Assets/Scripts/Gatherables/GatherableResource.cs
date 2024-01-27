using System;
using FGJ24.Inventory;
using UnityEngine;

namespace FGJ24.Gatherables
{
    public class GatherableResource : MonoBehaviour
    {
        [SerializeField] private ResourceType resourceType;

        [SerializeField] private int ResourceQuantity;

        public bool isGatherable = false;

        public ResourceType ResourceType => resourceType;

        public bool TakeResource()
        {
            if (ResourceQuantity <= 0)
            {
                isGatherable = false;
                return false;
            };

            if (!isGatherable) return false;

            ResourceQuantity -= 1;
            
            return true;
        }
    }
}
