using System;
using UnityEngine;

namespace FGJ24.Gatherables
{
    [RequireComponent(typeof(Collider))]
    public class GatherableResourceTriggerZone : MonoBehaviour
    {
        private Collider _collider;
        [SerializeField] private GatherableResource gatherable;

        private void Awake()
        {
            _collider = this.GetComponent<Collider>();
        }
        
        private void OnTriggerEnter(Collider other)
        {
            var gatherer = other.GetComponent<Gatherer>();
            if (gatherer == null) return;
            gatherable.isGatherable = true;
            gatherer.GatherableIsInRange(gatherable);

        }

        private void OnTriggerExit(Collider other)
        {
            var gatherer = other.GetComponent<Gatherer>();
            if (gatherer == null) return;
            gatherable.isGatherable = false;
            gatherer.GatherableNotInRange(gatherable);
        }
    }
}
