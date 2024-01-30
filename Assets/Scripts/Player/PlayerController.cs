using System;
using Unity.VisualScripting.ReorderableList;
using UnityEngine;

namespace FGJ24.Player
{
    [Serializable]
    public class PlayerController
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Transform _cameraTransform;
        
        [SerializeField] private bool _haveWeWon;
        [SerializeField] private bool _haveWeLost;

        [SerializeField] private float _maxGroundAngle = 45f;

        [SerializeField] private LayerMask _stairMask = -1;
        [SerializeField] private LayerMask _probeMask = -1;

        [SerializeField] private float _maxStairsAngle = 75f;
        [SerializeField] private float _maxSteepAngle = 75f;
        
        [SerializeField] private float _maxSnapSpeed = 100f;
        [SerializeField] private float _probeDistance = 1f;
        [SerializeField] private Vector3 _velocity;
        public bool IsGrounded => _groundContactCount > 0;
        public bool IsSteep => _steepContactCount > 0;

        [SerializeField] private float _jumpHeight = 5f;

        [SerializeField] private int _groundContactCount;
        [SerializeField] private int _steepContactCount;
        [SerializeField] private float _minGroundDotProduct;
        [SerializeField] private float _minStairsDotProduct;
        [SerializeField] private float _minSteepDotProduct;
        [SerializeField] private Vector3 _contactNormal;
        [SerializeField] private Vector3 _steepNormal;

        [SerializeField] private int _stepsSinceLastGrounded;
        [SerializeField] private int _stepsSinceLastJump;

        [SerializeField] private Vector3 _lastMovementDirection;

        [SerializeField] private Vector3 _desiredVelocity;


        [SerializeField] private int _jumpPhase;
        [SerializeField] private bool _intentToJump;
        [SerializeField] private bool _intentToDash;
        [SerializeField] private int _maxAirJumps;
        [SerializeField] private float _nextDashTime;
        [SerializeField] private float _dashStartTime;
        [SerializeField] private float _landingTime;
        [SerializeField] private float _nextJumpTime;



        public bool SnapToGround()
        {
            if (_stepsSinceLastGrounded > 1 || _stepsSinceLastJump <= 2)
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
            //Draw ray cast
            Debug.DrawRay(_rigidbody.position, Vector3.down * _probeDistance, Color.red, 2f);

            if (hit.normal.y < GetMinDot(hit.collider.gameObject.layer))
            {
                return false;
            }

            _groundContactCount = 1;
            _contactNormal = hit.normal;

            float dot = Vector3.Dot(_velocity, hit.normal);
            if (dot > 0f)
            {
                _velocity = (_velocity - hit.normal * dot).normalized * speed;
            }

            return true;
        }

        public float GetMinDot(int gameObjectLayer)
        {
            return (_stairMask & (1 << gameObjectLayer)) == 0 ? _minGroundDotProduct : _minStairsDotProduct;
        }

        public void CalculateMinGroundDotProduct()
        {
            _minGroundDotProduct = Mathf.Cos(_maxGroundAngle * Mathf.Deg2Rad);
        }

        public void CalculateMinStairsDotProduct()
        {
            _minStairsDotProduct = Mathf.Cos(_maxStairsAngle * Mathf.Deg2Rad);
        }
        
        public void CalculateMinSteepDotProduct()
        {
            _minSteepDotProduct = Mathf.Cos(_maxSteepAngle * Mathf.Deg2Rad);
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

        public void DashMoveVelocity(Vector3 direction, float speed)
        {
            _velocity += direction * speed;
        }

        public void Jump(float jumpHeight)
        {
            if (_intentToJump)
            {
                _intentToJump = false;
                Vector3 jumpDirection;
                if (GetIsGrounded())
                {
                    jumpDirection = GetContactNormal();
                }
                else if (GetIsSteep())
                {
                    jumpDirection = GetSteepNormal();
                    SetJumpPhase(0);
                }
                else if (GetMaxAirJumps() > 0 && GetJumpPhase() <= GetMaxAirJumps())
                {
                    if (GetJumpPhase() == 0)
                    {
                        SetJumpPhase(1);
                    }

                    jumpDirection = GetContactNormal();
                }
                else
                {
                    return;
                }


                _stepsSinceLastJump = 0;
                _jumpPhase += 1;
                float jumpSpeed = Mathf.Sqrt(-2f * Physics.gravity.y * jumpHeight);
                jumpDirection = (jumpDirection + Vector3.up).normalized;
                float alignedSpeed = Vector3.Dot(_velocity, jumpDirection);

                if (alignedSpeed > 0f)
                {
                    jumpSpeed = Mathf.Max(jumpSpeed - alignedSpeed, 0f);
                }

                _velocity += Vector3.up * jumpSpeed;
            }
        }

        public float GetNextJumpTime()
        {
            return _nextJumpTime;
        }

        public void SetNextJumpTime(float nextJumpTime)
        {
            _nextJumpTime = nextJumpTime;
        }

        public void SetLastMovementDirection(Vector3 lastMovementDirection)
        {
            if (_lastMovementDirection != lastMovementDirection && lastMovementDirection != Vector3.zero)
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

        public Vector3 GetRigidbodyVelocity()
        {
            return _rigidbody.velocity;
        }

        public Vector3 ProjectOnContactPlane(Vector3 vector)
        {
            return vector - _contactNormal * Vector3.Dot(vector, _contactNormal);
        }

        public void SetMaxSnapSpeed(float maxSnapSpeed)
        {
            _maxSnapSpeed = maxSnapSpeed;
        }

        public void AdjustVelocity(Vector3 velocity, float acceleration, Vector3 desiredVelocity)
        {
            Vector3 xAxis = ProjectOnContactPlane(Vector3.right).normalized;
            Vector3 zAxis = ProjectOnContactPlane(Vector3.forward).normalized;

            float currentX = Vector3.Dot(velocity, xAxis);
            float currentZ = Vector3.Dot(velocity, zAxis);

            float maxSpeedChange = acceleration * Time.deltaTime;

            float newX = Mathf.MoveTowards(currentX, desiredVelocity.x, maxSpeedChange);
            float newZ = Mathf.MoveTowards(currentZ, desiredVelocity.z, maxSpeedChange);
            
            _velocity = velocity + xAxis * (newX - currentX) + zAxis * (newZ - currentZ);
        }

        public void LimitVelocity(float maxSpeed)
        {
            float speed = _velocity.magnitude;
            if (speed > maxSpeed)
            {
                _velocity *= maxSpeed / (speed + Mathf.Epsilon);
            }
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
                else if (normal.y > _minSteepDotProduct)
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
            if (_steepContactCount > 1)
            {
                _contactNormal.Normalize();
                if (_contactNormal.y >= _minGroundDotProduct && _contactNormal.y <= _minSteepDotProduct)
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
                if (_stepsSinceLastJump > 1)
                {
                    _jumpPhase = 0;
                }

                if (_groundContactCount > 1)
                    _contactNormal.Normalize();
            }
            else
            {
                _contactNormal = Vector3.up;
            }
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

        public Vector3 GetVelocity()
        {
            return _velocity;
        }

        public void SetVelocity(Vector3 velocity)
        {
            _velocity = velocity;
        }


        public Vector3 GetDesiredVelocity()
        {
            return _desiredVelocity;
        }

        public void SetDesiredVelocity(Vector3 desiredVelocity)
        {
            _desiredVelocity = desiredVelocity;
        }

        public void UpdateDesiredVelocity(float speed)
        {
            Vector3 moveDirection = new Vector3(PlayerControls.Instance.moveData.moveValue.x, 0, PlayerControls.Instance.moveData.moveValue.y);
            moveDirection = _cameraTransform.forward * moveDirection.z + _cameraTransform.right * moveDirection.x;
            moveDirection.y = 0;
            _desiredVelocity = moveDirection * speed;
        }

        public bool GetIntentToJump()
        {
            return _intentToJump;
        }
        public void SetIntentToJump(bool intentToJump)
        {
            _intentToJump = intentToJump;
        }

        public void Dash(float speed)
        {
            Vector3 moveDirection = new Vector3(PlayerControls.Instance.moveData.moveValue.x, 0, PlayerControls.Instance.moveData.moveValue.y);
            moveDirection = _cameraTransform.forward * moveDirection.z + _cameraTransform.right * moveDirection.x;
            moveDirection.y = 0;
            _velocity += moveDirection * speed;
        }
        
        public bool GetIntentToDash()
        {
            return _intentToDash;
        }
        public void SetIntentToDash(bool intentToDash)
        {
            _intentToDash = intentToDash;
        }
        
        public float GetDashStartTime()
        {
            return _dashStartTime;
        }
        
        public void SetDashStartTime(float dashStartTime)
        {
            _dashStartTime = dashStartTime;
        }

        public bool HaveWeWon()
        {
            return _haveWeWon;
        }
        
        public bool HaveWeLost()
        {
            return _haveWeLost;
        }

        public void SetHaveWeWon(bool haveWeWon)
        {
            _haveWeWon = haveWeWon;
        }
        
        public void SetLandingTime(float landingTime)
        {
            _landingTime = landingTime;
        }
        public float GetLandingTime()
        {
            return _landingTime;
        }
    }
}