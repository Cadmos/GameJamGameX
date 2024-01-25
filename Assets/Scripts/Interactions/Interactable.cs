using System;
using Ioni.Extensions;
using UnityEngine;

namespace FGJ24.Interactions
{
    public class Interactable : MonoBehaviour
    {
        protected PlayerInteractions PlayerInteractions;
        
        protected void OnTriggerEnter(Collider other)
        {
            PlayerInteractions = other.gameObject.GetComponent<PlayerInteractions>();

            if (PlayerInteractions != null)
            {
                PlayerInteractions.RegisterInteractable(this, transform.position);
            }
        }

        protected void OnTriggerExit(Collider other)
        {
            PlayerInteractions = other.gameObject.GetComponent<PlayerInteractions>();

            if (PlayerInteractions != null)
            {
                PlayerInteractions.DeregisterInteractable(this);
            }
        }

        public virtual void Interact()
        {
            "No interactions registered here!".Warn();
        }
    }
}
