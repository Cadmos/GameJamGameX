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

        [SerializeField] private Transform _groundCheck;
        [SerializeField] private LayerMask _groundMask;
        [SerializeField] private float _groundDistance = 0.4f;
        [SerializeField] private float _maxGroundAngle = 60f;

        [SerializeField] private bool _wasGroundedLastFrame;

        [SerializeField] private Vector3 _lastMovementDirection;
        [SerializeField] private float _dashCooldownTimer;

        [SerializeField] private float _nextDashTime;
        [SerializeField] private float _nextJumpTime;




        public Transform GetCameraTransform()
        {
            return _cameraTransform;
        }

        public bool IsGrounded()
        {
            RaycastHit hit;
            if (Physics.Raycast(_groundCheck.position, Vector3.down, out hit, _groundDistance, _groundMask))
            {
                Debug.DrawRay(_groundCheck.position, Vector3.down * (_groundDistance + 1), Color.red, 2f);
                float groundAngle = Vector3.Angle(hit.normal, Vector3.up);
                if (groundAngle <= _maxGroundAngle)
                {
                    _wasGroundedLastFrame = true;
                    return true;
                }
            }

            return false;
        }

        public bool WasGroundedLastFrame()
        {
            return _wasGroundedLastFrame;
        }

        public void Move(Vector3 direction, float movementSpeed)
        {
            _rigidbody.MovePosition(_rigidbody.position + direction * (movementSpeed * Time.deltaTime));
            //_rigidbody.velocity = direction * (movementSpeed * Time.deltaTime);
        }

        public void StopHorizontalMovement()
        {
            _rigidbody.velocity = new Vector3(0, _rigidbody.velocity.y, 0);
        }
        public void AirMove(Vector3 momentum, Vector3 direction, float speed, float airControl)
        {
            Vector3 targetPosition = _rigidbody.position + momentum + direction * (speed * airControl);
            Debug.Log("speed: " + speed);
            Debug.Log("airControl: " + airControl);
            
            _rigidbody.MovePosition(targetPosition);
        }

        public void Jump(float jumpForce)
        {
            _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        public float GetNextJumpTime()
        {
            return _nextJumpTime;
        }

        public void SetNextJumpTime(float nextJumpTime)
        {
            _nextJumpTime = nextJumpTime;
        }

        public void Dash(float dashSpeed)
        {
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

        public void SetLastMovementDirection(Vector3 lastMovementDirection)
        {
            if (_lastMovementDirection != lastMovementDirection && lastMovementDirection != Vector3.zero)
                _lastMovementDirection = lastMovementDirection.normalized;
        }
        public Vector3 GetLastMovementDirection()
        {
            return _lastMovementDirection;
        }

        public float GetNextDashTime()
        {
            return _nextDashTime;
        }

        public void SetNextDashTime(float nextDashTime)
        {
            _nextDashTime = nextDashTime;
        }

        public void SetWasGroundedLastFrame(bool wasGroundedLastFrame)
        {
            _wasGroundedLastFrame = wasGroundedLastFrame;
        }
        
        public bool GetWasGroundedLastFrame()
        {
            return _wasGroundedLastFrame;
        }

        public Vector3 GetRigidbodyVelocity()
        {
            return _rigidbody.velocity;
        }
    }
}