using System;
using UnityEngine;

namespace FGJ24.Pylons
{
    [RequireComponent(typeof(SphereCollider))]
    public class Pulser : MonoBehaviour
    {
        [SerializeField] private Transform graphicalPulse;
        
        private SphereCollider _pulseCollider;
        private bool _isPulsing = false;
        private Pylon _pylon;
        private void Awake()
        {
            _pulseCollider = GetComponent<SphereCollider>();
            _pylon = GetComponentInParent<Pylon>();
        }

        private void Start()
        {
            _isPulsing = true; // TODO - Temporarily enable pulsing
        }

        private void StartPulse()
        {
            _isPulsing = true;
        }

        private void Update()
        {
            if (!_isPulsing) return;
            _pulseCollider.radius = _pylon.CurrentValue;
            graphicalPulse.localScale = new Vector3(1+_pylon.CurrentValue, 1+_pylon.CurrentValue, 1+_pylon.CurrentValue) ;
        }
    }
}
