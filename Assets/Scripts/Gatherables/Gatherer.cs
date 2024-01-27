using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FGJ24.Gatherables
{
    public class Gatherer : MonoBehaviour
    {
        [SerializeField] private List<GatherableResource> gatherablesInRange;
        [SerializeField] private Inventory.Inventory inventory;

        public void GatherableIsInRange(GatherableResource gatherableResource)
        {
            gatherablesInRange.Add(gatherableResource);
        }

        public void GatherableNotInRange(GatherableResource gatherableResource)
        {
            gatherablesInRange.Remove(gatherableResource);
        }

        private void Update()
        {
            // TODO - Not Like This. Prefer unity input system. 
            if (!Input.GetKeyDown(KeyCode.R)) return;

            TryGather();
        }

        private void TryGather()
        {
            var firstGatherable = gatherablesInRange.First();
            if (firstGatherable == null) return;
            if (!firstGatherable.isGatherable) return;
            
            var success = firstGatherable.TakeResource();
            if(success)
                inventory.AddResource(firstGatherable.ResourceType);
        }
    }
}
