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
        
        private Vector3 _startScale = Vector3.zero;
        [SerializeField] private float scaleModifier;
        
        private void Awake()
        {
            _pulseCollider = GetComponent<SphereCollider>();
            _pylon = GetComponentInParent<Pylon>();
        }

        private void Start()
        {
            _startScale = transform.localScale;
            _isPulsing = true; // TODO - Temporarily enable pulsing
        }

        private void StartPulse()
        {
            _isPulsing = true;
        }

        private void Update()
        {
            if (!_isPulsing) return;
            //_pulseCollider.radius = _pylon.CurrentValue;
            // transform.localScale = CalculateScale();
            transform.localScale = new Vector3(1+_pylon.CurrentValue, 1+_pylon.CurrentValue, 1+_pylon.CurrentValue) ;
        }

        private Vector3 CalculateScale()
        {
            return _startScale * scaleModifier;
        }
    }
}
