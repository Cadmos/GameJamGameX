using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FGJ24.Interactions
{
    [Serializable]
    public struct InteractableData
    {
        public Vector3 position;
        public Interactable Interactable;
    }
    
    public class PlayerInteractions : MonoBehaviour
    {
        [SerializeField] private List<InteractableData> _interactablesInRange;

        public void RegisterInteractable(Interactable i, Vector3 position)
        {
            _interactablesInRange.Add(new InteractableData { position = position, Interactable = i });
        }

        public void DeregisterInteractable(Interactable i)
        {
            _interactablesInRange.Remove(_interactablesInRange.Find(interactable => interactable.Interactable == i));
        }

        private Interactable ClosestInteractable()
        {
            var ownPosition = transform.position;
            IEnumerable<InteractableData> orderedInteractables = _interactablesInRange.OrderBy(interactableData => Vector3.Distance(interactableData.position, ownPosition));
            return orderedInteractables.First().Interactable;
        }

        public void LaunchInteraction()
        {
            ClosestInteractable().Interact();
        }
    }
}
