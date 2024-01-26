using System;
using UnityEngine;

namespace FGJ24.Player
{
    [Serializable]
    public class PlayerController
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Collider _collider;
        [SerializeField] private Transform _cameraTransform;
        
        [SerializeField] private Vector3 _lastMovementDirection;
        [SerializeField] private float _dashCooldownTimer;
        [SerializeField] private float _dashCooldownDuration;

        [SerializeField] private float _nextDashTime;
        [SerializeField] private bool _intentToDash;


        
        public Transform GetCameraTransform()
        {
            return _cameraTransform;
        }

        public void Initialize()
        {
            
        }
        
        public void Move(Vector3 direction, float movementSpeed)
        {
            _rigidbody.MovePosition(_rigidbody.position + direction * (movementSpeed * Time.deltaTime));
        }
        
        public void Dash(float dashSpeed)
        {
            Debug.Log("PlayerController.Dash");
            _rigidbody.AddForce(_lastMovementDirection * dashSpeed, ForceMode.Impulse);
        }
        
        
        public float GetDashCooldownTimer()
        {
            return _dashCooldownTimer;
        }
        public void SetDashCooldownTimer(float dashCooldownTimer)
        {
            _dashCooldownTimer = dashCooldownTimer;
        }
        
        public void SetDashCooldownDuration(float dashCooldownDuration)
        {
            _dashCooldownDuration = dashCooldownDuration;
        }
        
        public bool GetIntentToDash()
        {
            return _intentToDash;
        }

        public void SetIntentToDash(bool intentToDash)
        {
            _intentToDash = intentToDash;
        }
        
        public void SetLastMovementDirection(Vector3 lastMovementDirection)
        {
            if(_lastMovementDirection != lastMovementDirection && lastMovementDirection != Vector3.zero)
            _lastMovementDirection = lastMovementDirection.normalized;
        }

        public float GetNextDashTime()
        {
            return _nextDashTime;
        }
        
        public void SetNextDashTime(float nextDashTime)
        {
            _nextDashTime = nextDashTime;
        }
        
        
    }
}