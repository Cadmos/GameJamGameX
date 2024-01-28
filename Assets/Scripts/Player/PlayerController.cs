using System;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace FGJ24.Player
{
    [Serializable]
    public class PlayerController
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Transform _cameraTransform;
        
        [SerializeField] private float _maxGroundAngle = 45f;
        
        [SerializeField] private LayerMask _stairMask = -1;
        [SerializeField] private LayerMask _probeMask = -1;
        
        [SerializeField] private float _maxStairsAngle = 75f;
        [SerializeField] private float _maxSnapSpeed = 100f;
        [SerializeField] private float _probeDistance = 1f;
        [SerializeField] private Vector3 _velocity;
        public bool IsGrounded => _groundContactCount > 0;
        public bool IsSteep => _steepContactCount > 0;
        [SerializeField] private bool _wasGroundedLastFrame;

        [SerializeField] private float _jumpHeight = 5f;
        
        [SerializeField] private int _groundContactCount;
        [SerializeField] private int _steepContactCount;
        [SerializeField] private float _minGroundDotProduct;
        [SerializeField] private float _minStairsDotProduct;
        [SerializeField] private Vector3 _contactNormal;
        [SerializeField] private Vector3 _steepNormal;

        [SerializeField] private int _stepsSinceLastGrounded;
        [SerializeField] private int _stepsSinceLastJump;
        
        [SerializeField] private Vector3 _lastMovementDirection;
        [SerializeField] private float _dashCooldownTimer;

        [SerializeField] private Vector3 _desiredVelocity;
        
        [SerializeField] private float _maxAcceleration = 10f;
        
        [SerializeField] private float _maxAirAcceleration = 1f;
        
        [SerializeField] private float _maxAirHorizontalSpeed = 10f;
        
        [SerializeField] private float _maxAirVerticalSpeed = 10f;
        
        [SerializeField] private int _jumpPhase;
        
        [SerializeField] private int _maxAirJumps = 0;
        [SerializeField] private float _nextDashTime;
        [SerializeField] private float _nextJumpTime;
        

        public bool SnapToGround()
        {
            if(_stepsSinceLastGrounded>1 || _stepsSinceLastJump <= 2)
            {
                return false;
            }
            float speed = _velocity.magnitude;
            //Debug.Log("speed: " + speed + " _maxSnapSpeed: " + _maxSnapSpeed);
            if (speed > _maxSnapSpeed)
            {
                return false;
            }
            if (!Physics.Raycast(_rigidbody.position, Vector3.down, out RaycastHit hit, _probeDistance, _probeMask))
            {
                return false;
            }
            if (hit.normal.y < GetMinDot(hit.collider.gameObject.layer))
            {
                return false;
            }
            
            _groundContactCount = 1;
            _contactNormal = hit.normal;
            
            float dot = Vector3.Dot(_velocity, hit.normal);
            if (dot > 0f)
            {
                _velocity = ( _velocity - hit.normal * dot).normalized * speed;
            }
            return true;
        }

        public float GetMinDot(int gameObjectLayer)
        {
            return(_stairMask & (1 << gameObjectLayer)) == 0 ? _minGroundDotProduct : _minStairsDotProduct;
        }

        public void CalculateMinGroundDotProduct()
        {
            _minGroundDotProduct = Mathf.Cos(_maxGroundAngle * Mathf.Deg2Rad);
        }
        public void CalculateMinStairsDotProduct()
        {
            _minStairsDotProduct = Mathf.Cos(_maxStairsAngle * Mathf.Deg2Rad);
        }
        public void RecordVelocity()
        {
            _velocity = _rigidbody.velocity;
        }
        public void SetStepsSinceLastGrounded(int stepsSinceLastGrounded)
        {
            _stepsSinceLastGrounded = stepsSinceLastGrounded;
        }
        
        
        public void AddToStepsSinceLastGrounded(int stepsSinceLastGrounded)
        {
            _stepsSinceLastGrounded += stepsSinceLastGrounded;
        }
        public int GetStepsSinceLastGrounded()
        {
            return _stepsSinceLastGrounded;
        }

        public bool GetIsGrounded()
        {
            return IsGrounded;
        }
        
        public bool GetIsSteep()
        {
            return IsSteep;
        }
        public Transform GetCameraTransform()
        {
            return _cameraTransform;
        }
        
        public Vector3 GetContactNormal()
        {
            return _contactNormal;
        }
        public Vector3 GetSteepNormal()
        {
            return _steepNormal;
        }
        public void SetJumpPhase(int jumpPhase)
        {
            _jumpPhase = jumpPhase;
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
        
        
        public void IdleVelocity()
        {
            _rigidbody.velocity = Vector3.zero;
        }
        
        public void MoveVelocity(Vector3 direction, float movementSpeed)
        {
            _rigidbody.velocity = new Vector3(direction.x*movementSpeed, _rigidbody.velocity.y, direction.z*movementSpeed);
        }
        
        public void AirMoveVelocity(Vector3 momentum, Vector3 direction, float speed, float airControl)
        {
            _rigidbody.velocity = new Vector3(direction.x*speed*airControl, _rigidbody.velocity.y, direction.z*speed*airControl);
        }
        
        public void DashMoveVelocity(Vector3 direction, float speed)
        {
            _velocity += direction * speed;
        }

        
        public void StopHorizontalMovement()
        {
            _rigidbody.velocity = new Vector3(0, _rigidbody.velocity.y, 0);
        }
        public void AirMove(Vector3 momentum, Vector3 direction, float speed, float airControl)
        {
            Vector3 targetPosition = _rigidbody.position + momentum + direction * (speed * airControl);
            //Debug.Log("speed: " + speed);
            //Debug.Log("airControl: " + airControl);
            
            _rigidbody.MovePosition(targetPosition);
        }

        public void Jump(float jumpForce)
        {
            _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        public void Jump2(Vector3 jumpDirection)
        {
            _stepsSinceLastJump = 0;
            _jumpPhase += 1;
            float jumpSpeed = Mathf.Sqrt(-2f * Physics.gravity.y * _jumpHeight);
            jumpDirection = (jumpDirection + Vector3.up).normalized;
            float alignedSpeed = Vector3.Dot(_velocity, jumpDirection);
            
            if (alignedSpeed > 0f)
            {
                jumpSpeed = Mathf.Max(jumpSpeed - alignedSpeed, 0f);
            }
            
            _velocity += Vector3.up * jumpSpeed;
        }

        public void JumpVelocity(float jumpHeight)
        {
            _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y), _rigidbody.velocity.z);
        }
        
        public void JumpMoveVelocity( Vector3 direction, float speed)
        {
            _rigidbody.velocity = new Vector3(direction.x*speed, _rigidbody.velocity.y, direction.z*speed);
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
        
        
        public void SetContactNormal(Vector3 contactNormal)
        {
            _contactNormal = contactNormal;
        }
        
        public Vector3 ProjectOnContactPlane(Vector3 vector)
        {
            return vector - _contactNormal * Vector3.Dot(vector, _contactNormal);
        }
        
        public void AdjustVelocityAlongSurface()
        {
            Vector3 xAxis = ProjectOnContactPlane(Vector3.right).normalized;
            Vector3 zAxis = ProjectOnContactPlane(Vector3.forward).normalized;

            float currentX = Vector3.Dot(_velocity, xAxis);
            float currentZ = Vector3.Dot(_velocity, zAxis);
            
            float acceleration = GetIsGrounded() ? _maxAcceleration : _maxAirAcceleration;
            float maxSpeedChange = acceleration * Time.deltaTime;
            
            float newX = Mathf.MoveTowards(currentX, _desiredVelocity.x, maxSpeedChange);
            float newZ = Mathf.MoveTowards(currentZ, _desiredVelocity.z, maxSpeedChange);
            
            _velocity += xAxis * (newX - currentX) + zAxis * (newZ - currentZ);
        }
        
        public void SetMaxSnapSpeed(float maxSnapSpeed)
        {
            _maxSnapSpeed = maxSnapSpeed;
        }
        
        public void AdjustVelocityAlongSurface(float acceleration)
        {
            Vector3 xAxis = ProjectOnContactPlane(Vector3.right).normalized;
            Vector3 zAxis = ProjectOnContactPlane(Vector3.forward).normalized;

            float currentX = Vector3.Dot(_velocity, xAxis);
            float currentZ = Vector3.Dot(_velocity, zAxis);
            
            float maxSpeedChange = acceleration * Time.deltaTime;
            
            float newX = Mathf.MoveTowards(currentX, _desiredVelocity.x, maxSpeedChange);
            float newZ = Mathf.MoveTowards(currentZ, _desiredVelocity.z, maxSpeedChange);
            
            _velocity += xAxis * (newX - currentX) + zAxis * (newZ - currentZ);
        }
        
        
        
        public void EvaluateCollisions(Collision collision)
        {
            //Debug.Log("EvaluateCollisions");
            float minDot = GetMinDot(collision.gameObject.layer);
            for (int i = 0; i < collision.contactCount; i++)
            {
                Vector3 normal = collision.GetContact(i).normal;
                if (normal.y >= minDot)
                {
                    _groundContactCount += 1;
                    _contactNormal += normal;
                }
                else if(normal.y > -0.01f)
                {
                    _steepContactCount += 1;
                    _steepNormal += normal;
                }
            }
        }

        public void ClearState()
        {
            _groundContactCount = _steepContactCount = 0;
            _contactNormal = _steepNormal = Vector3.zero;
        }

        public bool CheckSteepContacts()
        {
            if(_steepContactCount > 1)
            {
                _contactNormal.Normalize();
                if (_contactNormal.y >= _minGroundDotProduct)
                {
                    _groundContactCount = 1;
                    _steepContactCount = 0;
                    _contactNormal = _steepNormal;
                    return true;
                }
            }
            return false;
        }
        public void UpdateState()
        {
            _stepsSinceLastGrounded += 1;
            _stepsSinceLastJump += 1;
            _velocity = _rigidbody.velocity;
            
            if (GetIsGrounded() || SnapToGround() || CheckSteepContacts())
            {
                _stepsSinceLastGrounded = 0;
                if(_stepsSinceLastJump > 1)
                {
                    _jumpPhase = 0;
                }
                if(_groundContactCount > 1)
                    _contactNormal.Normalize();
            }
            else
            {
                _contactNormal = Vector3.up;
            }
        }
        
        public void SetDesiredVelocity(Vector3 desiredVelocity)
        {
            _desiredVelocity = desiredVelocity;
        }

        public void UpdateRigidBodyVelocity()
        {
            _rigidbody.velocity = _velocity;
        }

        public int GetMaxAirJumps()
        {
            return _maxAirJumps;
        }
        
        public int GetJumpPhase()
        {
            return _jumpPhase;
        }

        public bool GetIsDashing()
        {
            return _nextDashTime <= Time.time;
        }
    }
}