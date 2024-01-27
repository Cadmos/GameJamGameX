using System;
using System.Collections.Generic;
using System.Linq;
using FGJ24.Player;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace FGJ24.Interactions
{
    [Serializable]
    public struct InteractableData
    {
        public Vector3 interactableObjectPosition;
        public Interactable interactable;
    }
    
    public class PlayerInteractions : MonoBehaviour
    {
        [SerializeField] private List<InteractableData> interactablesInRange;

        [SerializeField] private PlayerControls playerControls;

        private void Start()
        {
            playerControls.SubscribeInteraction(this);
        }

        public void RegisterInteractable(Interactable i, Vector3 position)
        {
            interactablesInRange.Add(new InteractableData { interactableObjectPosition = position, interactable = i });
        }

        public void DeregisterInteractable(Interactable i)
        {
            interactablesInRange.Remove(interactablesInRange.Find(interactable => interactable.interactable == i));
        }

        private Interactable ClosestInteractable()
        {
            var ownPosition = transform.position;
            IEnumerable<InteractableData> orderedInteractables = interactablesInRange.OrderBy(interactableData => Vector3.Distance(interactableData.interactableObjectPosition, ownPosition));
            return orderedInteractables.First().interactable;
        }

        public void LaunchInteraction(InputAction.CallbackContext ctx)
        {
            ClosestInteractable().Interact();
        }
    }
}
