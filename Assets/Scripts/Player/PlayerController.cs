using System;
using FGJ24.CustomPhysics;
using UnityEngine;

namespace FGJ24.Player
{
    [Serializable]
    public class PlayerController
    {
        #region Properties
        [SerializeField] private Transform _playerInputSpace;
        [SerializeField] private Transform _cameraTransform;
        [SerializeField] private Transform _upAxisTransform;

        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Rigidbody _connectedRigidbody;
        [SerializeField] private Rigidbody _previousConnectedRigidbody;

        [SerializeField] private CapsuleCollider _collider;

        [SerializeField] private Quaternion _targetRotation;
        [SerializeField] private Quaternion _gravityAlignment = Quaternion.identity;

        [SerializeField] private Vector3 _upAxis;
        [SerializeField] private Vector3 _rightAxis;
        [SerializeField] private Vector3 _forwardAxis;
        [SerializeField] private Vector3 _gravity;
        [SerializeField] private Vector3 _velocity;
        [SerializeField] private Vector3 _desiredVelocity;
        [SerializeField] private Vector3 _connectionVelocity;
        [SerializeField] private Vector3 _position;
        [SerializeField] private Vector3 _connectionWorldPosition;
        [SerializeField] private Vector3 _connectionLocalPosition;
        [SerializeField] private Vector3 _contactNormal;
        [SerializeField] private Vector3 _steepNormal;
        [SerializeField] private Vector3 _climbNormal;
        [SerializeField] private Vector3 _snapContactNormal;
        [SerializeField] private Vector3 _lastClimbNormal;

        [SerializeField] private LayerMask _stairMask = -1;
        [SerializeField] private LayerMask _probeMask = -1;
        [SerializeField] private LayerMask _climbMask = -1;
        [SerializeField] private LayerMask _waterMask = -1;

        [SerializeField] private float _upAlignmentSpeed = 360f;
        [SerializeField] private float _minGroundDotProduct;
        [SerializeField] private float _minStairsDotProduct;
        [SerializeField] private float _minSteepDotProduct;
        [SerializeField] private float _minClimbDotProduct;
        [SerializeField] private float _maxSnapSpeed = 100f;
        [SerializeField] private float _probeDistance = 1f;
        [SerializeField] private float _distanceFromGround;
        [SerializeField] private float _stepSmooth = 0.2f;
        [SerializeField] private float _nextDashTime;
        [SerializeField] private float _dashStartTime;
        [SerializeField] private float _landingTime;
        [SerializeField] private float _nextJumpTime;

        [SerializeField, Range(1, 90)] private float _maxGroundAngle = 45f;
        [SerializeField, Range(1, 90)] private float _maxStairsAngle = 60f;
        [SerializeField, Range(1, 90)] private float _maxSteepAngle = 75f;
        [SerializeField, Range(90, 180)] private float _maxClimbAngle = 140f;

        [SerializeField] private int _jumpPhase;
        [SerializeField] private int _maxAirJumps;
        [SerializeField] private int _stepsSinceLastGrounded;
        [SerializeField] private int _stepsSinceLastJump;
        [SerializeField] private int _groundContactCount;
        [SerializeField] private int _steepContactCount;
        [SerializeField] private int _climbContactCount;
        [SerializeField] private float _submergence;

        [SerializeField] private float _submergenceOffset = 0.5f;
        [SerializeField, Min(0.1f)] private float _submergenceRange = 1f;
        [SerializeField, Range(0f, 10f)] private float _waterDrag = 1;
        [SerializeField, Min(0f)] private float _buoyancy = 1f;
        [SerializeField, Range(0.01f, 1f)] private float _swimThreshold = 0.5f;

        [SerializeField] private bool _isSnapping;
        [SerializeField] private bool _intentToJump;
        [SerializeField] private bool _intentToDash;
        [SerializeField] private bool _haveWeWon;
        [SerializeField] private bool _haveWeLost;

        public bool WasGroundedLastFrame => _stepsSinceLastGrounded <= 1;
        public bool IsSnapping => _isSnapping;
        public bool IsGrounded => _groundContactCount > 0;

        public bool OnSlope => _steepContactCount > 0;
        public bool IsSteep => _steepContactCount > 0;


        public bool TouchingWall => _climbContactCount > 0;
        public bool IsClimbing => _climbContactCount > 0 && _stepsSinceLastJump > 2;

        public bool InWater => _submergence > 0f;
        public bool Swimming => _submergence >= _swimThreshold;
        #endregion

        #region State Methods
        
        public void Jump(Vector3 velocity, float acceleration, Vector2 playerInput, float maxSpeed)
        {
            _stepsSinceLastJump = 0;
            velocity = FloatMovement(velocity, acceleration, playerInput, maxSpeed, _upAxis);

            AdjustVelocityDebug(velocity, playerInput, maxSpeed);
            
            velocity += _gravity * Time.deltaTime;
            _velocity = velocity;
        }
        
        public void Falling(Vector3 velocity, float acceleration, Vector2 playerInput, float maxSpeed)
        {
            velocity = FloatMovement(velocity, acceleration, playerInput, maxSpeed, _upAxis);

            AdjustVelocityDebug(velocity, playerInput, maxSpeed);

            velocity += _gravity * Time.deltaTime;

            _velocity = velocity;
        }

        public void Swim(Vector3 velocity, float acceleration, Vector2 playerInput, float maxSpeed)
        {
            velocity *= 1f - _waterDrag * _submergence * Time.deltaTime;
            velocity = HorizontalMovement(velocity, acceleration, playerInput, maxSpeed, _upAxis);
            velocity += _gravity * ((1f - _buoyancy * _submergence) * Time.deltaTime);

            _velocity = velocity;
        }

        public void Move(Vector3 velocity, float acceleration, Vector2 playerInput, float maxSpeed)
        {
            velocity = HorizontalMovement(velocity, acceleration, playerInput, maxSpeed, _contactNormal);

            if (_isSnapping)
            {
                if (_distanceFromGround > _stepSmooth)
                {
                    _position -= _upAxis * _stepSmooth;
                }
            }

            AdjustVelocityDebug(velocity, playerInput, maxSpeed);

            velocity += _contactNormal * (Vector3.Dot(_gravity, _contactNormal) * Time.deltaTime);

            velocity += _gravity * Time.deltaTime;
            _velocity = velocity;
        }

        public void Climbing(Vector3 velocity, float acceleration, Vector2 playerInput, float maxSpeed)
        {
            velocity = ClimbingMovement(velocity, acceleration, playerInput, maxSpeed, _contactNormal);

            if (_isSnapping)
            {
                if (_distanceFromGround > _stepSmooth)
                {
                    _position -= _upAxis * _stepSmooth;
                }
            }

            velocity -= _contactNormal * (acceleration * Time.deltaTime);

            AdjustVelocityDebug(velocity, playerInput, maxSpeed);
            _velocity = velocity;
        }

        public void Sliding(Vector3 velocity, float acceleration, Vector2 playerInput, float maxSpeed)
        {
            if (Vector3.Dot(velocity, _upAxis) > 0f)
            {
                velocity -= Vector3.Project(velocity, _upAxis);
            }

            float originalSpeed = velocity.magnitude;

            Vector3 newForward = ProjectDirectionOnContactPlane(-_upAxis, _contactNormal);

            float maxSpeedChange = acceleration * Time.deltaTime;

            float newSpeed = Mathf.MoveTowards(originalSpeed, maxSpeed, maxSpeedChange);


            velocity += newForward * newSpeed;

            velocity += _gravity * Time.deltaTime;

            if (_isSnapping)
            {
                if (_distanceFromGround > _stepSmooth)
                {
                    _position -= _upAxis * _stepSmooth;
                }
            }

            AdjustVelocityDebug(velocity, playerInput, maxSpeed);
            _velocity = velocity;
        }


        public void JumpToHeight(Vector3 velocity, Vector3 direction, float jumpHeight)
        {
                _stepsSinceLastJump = 0;
                _jumpPhase += 1;
                
                float jumpSpeed = Mathf.Sqrt(2f * _gravity.magnitude * jumpHeight);
                direction = (direction + _upAxis).normalized;
                
                float alignedSpeed = Vector3.Dot(velocity, direction);

                if (alignedSpeed > 0f)
                {
                    jumpSpeed = Mathf.Max(jumpSpeed - alignedSpeed, 0f);
                }

                _velocity += direction * jumpSpeed;
        }
        #endregion

        #region Debug

        void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            float radius = _collider.radius;
            float height = _collider.height;
            Vector3 center = _collider.center;
            Gizmos.DrawWireSphere(_position + center - _upAxis * radius, radius);
            Gizmos.DrawWireSphere(_position - _upAxis * radius + _upAxis * height, radius);
            Debug.DrawLine(_position, _position + -_upAxis * _probeDistance, Color.red, 10f);
            Debug.DrawLine(_position, _position + _upAxis * height, Color.magenta, 10f);
        }

        private void AdjustVelocityDebug(Vector3 velocity, Vector2 playerInput, float maxSpeed)
        {
            DebugTransform.Instance.SetForwardTransform(_upAxisTransform.forward);
            DebugTransform.Instance.SetRightTransform(_upAxisTransform.right);
            DebugTransform.Instance.SetUpTransform(_upAxisTransform.up);

            Debug.DrawLine(_rigidbody.position, _position + velocity, Color.green);
            Debug.DrawLine(_rigidbody.position, _position + new Vector3(playerInput.x * maxSpeed, 0, playerInput.y * maxSpeed), Color.red);
        }

        #endregion

        #region VelocityAdjustments

        public Vector3 FloatMovement(Vector3 velocity, float acceleration, Vector2 playerInput, float maxSpeed, Vector3 contactNormal)
        {
            Vector3 xAxis = ProjectDirectionOnContactPlane(_rightAxis, contactNormal);
            Vector3 zAxis = ProjectDirectionOnContactPlane(_forwardAxis, contactNormal);

            Vector3 relativeVelocity = velocity - _connectionVelocity;
            float currentX = Vector3.Dot(relativeVelocity, xAxis);
            float currentZ = Vector3.Dot(relativeVelocity, zAxis);

            float maxSpeedChange = acceleration * Time.deltaTime;

            float newX = Mathf.MoveTowards(currentX, playerInput.x * maxSpeed, maxSpeedChange);
            float newZ = Mathf.MoveTowards(currentZ, playerInput.y * maxSpeed, maxSpeedChange);

            return velocity + (xAxis * (newX - currentX) + zAxis * (newZ - currentZ));
        }

        public Vector3 HorizontalMovement(Vector3 velocity, float acceleration, Vector2 playerInput, float maxSpeed, Vector3 contactNormal)
        {
            Vector3 xAxis = ProjectDirectionOnContactPlane(_rightAxis, contactNormal);
            Vector3 zAxis = ProjectDirectionOnContactPlane(_forwardAxis, contactNormal);

            Vector3 relativeVelocity = velocity - _connectionVelocity;
            float currentX = Vector3.Dot(relativeVelocity, xAxis);
            float currentZ = Vector3.Dot(relativeVelocity, zAxis);

            float maxSpeedChange = acceleration * Time.deltaTime;

            float newX = Mathf.MoveTowards(currentX, playerInput.x * maxSpeed, maxSpeedChange);
            float newZ = Mathf.MoveTowards(currentZ, playerInput.y * maxSpeed, maxSpeedChange);

            return velocity + (xAxis * (newX - currentX) + zAxis * (newZ - currentZ));
        }

        public Vector3 ClimbingMovement(Vector3 velocity, float acceleration, Vector2 playerInput, float maxSpeed, Vector3 contactNormal)
        {
            Vector3 xAxis = Vector3.Cross(contactNormal, _upAxis);
            Vector3 zAxis = _upAxis;

            xAxis = ProjectDirectionOnContactPlane(xAxis, contactNormal);
            zAxis = ProjectDirectionOnContactPlane(zAxis, contactNormal);

            Vector3 relativeVelocity = velocity - _connectionVelocity;
            float currentX = Vector3.Dot(relativeVelocity, xAxis);
            float currentZ = Vector3.Dot(relativeVelocity, zAxis);

            float maxSpeedChange = acceleration * Time.deltaTime;

            float newX = Mathf.MoveTowards(currentX, playerInput.x * maxSpeed, maxSpeedChange);
            float newZ = Mathf.MoveTowards(currentZ, playerInput.y * maxSpeed, maxSpeedChange);

            return velocity + (xAxis * (newX - currentX) + zAxis * (newZ - currentZ));
        }

        public Vector3 VerticalMovement(Vector3 velocity, float acceleration, float desiredYVelocity, Vector3 contactNormal)
        {
            Vector3 yAxis = ProjectDirectionOnContactPlane(_upAxis, contactNormal).normalized;

            float currentY = Vector3.Dot(velocity, yAxis);
            float maxSpeedChange = acceleration * Time.deltaTime;

            float newY = Mathf.MoveTowards(currentY, desiredYVelocity, maxSpeedChange);
            return velocity + (yAxis * (newY - currentY));
        }

        public void LimitVelocity(float maxSpeed)
        {
            float speed = _velocity.magnitude;
            if (speed > maxSpeed)
            {
                _velocity *= maxSpeed / (speed + Mathf.Epsilon);
            }
        }

        #endregion

        #region Gravity

        public void UpdateGravity()
        {
            _gravity = CustomGravity.GetGravity(_position, out _upAxis);
            UpdateGravityAlignment();
            AlignCollider();
        }

        public void UpdateUpAxisTransform()
        {
            _upAxisTransform.up = _upAxis;
        }

        public void UpdateGravityAlignment()
        {
            Vector3 fromUp = _gravityAlignment * Vector3.up;
            Vector3 toUp = CustomGravity.GetUpAxis(_position);
            float dot = Mathf.Clamp(Vector3.Dot(fromUp, toUp), -1f, 1f);
            float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;
            float maxAngle = _upAlignmentSpeed * Time.deltaTime;
            Quaternion newAlignment = Quaternion.FromToRotation(fromUp, toUp) * _gravityAlignment;

            _gravityAlignment = angle <= maxAngle ? newAlignment : Quaternion.SlerpUnclamped(_gravityAlignment, newAlignment, maxAngle / angle);

            _upAxisTransform.rotation = _gravityAlignment;
            _rightAxis = ProjectDirectionOnContactPlane(_cameraTransform.right, _upAxis);
            _forwardAxis = ProjectDirectionOnContactPlane(_cameraTransform.forward, _upAxis);
        }

        public void AlignCollider()
        {
            _collider.transform.rotation = Quaternion.LookRotation(_forwardAxis, _upAxis);
        }

        #endregion

        #region PhysicsStateUpdates

        public void PrePhysicsUpdate()
        {
            _stepsSinceLastGrounded += 1;
            _stepsSinceLastJump += 1;
            _position = _rigidbody.position;
            _velocity = _rigidbody.velocity;
            if (CheckClimbing() || CheckSwimming() || GetIsGrounded() || SnapToGround() || CheckSteepContacts())
            {
                if (_isSnapping)
                {
                    _contactNormal = _snapContactNormal != Vector3.zero ? _snapContactNormal : _upAxis;
                    float dot = Vector3.Dot(_velocity, _contactNormal);
                    if (dot > 0f)
                    {
                        _velocity = (_velocity - _contactNormal * dot).normalized * _velocity.magnitude;
                    }
                }

                if (_contactNormal == Vector3.zero)
                {
                    _contactNormal = _upAxis;
                }

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
                Debug.Log("Checks fell through!");
                _contactNormal = _upAxis;
            }
            Debug.Log(this + "Contact normal: " + _contactNormal);

            if (_connectedRigidbody)
            {
                if (_connectedRigidbody.isKinematic || _connectedRigidbody.mass >= _rigidbody.mass)
                {
                    UpdateConnectionState();
                }
            }
        }

        private bool CheckSwimming()
        {
            if (Swimming)
            {
                _groundContactCount = 0;
                _contactNormal = _upAxis;
                Debug.Log("CheckSwimming() check returned true");
                return true;
            }

            return false;
        }

        private bool CheckClimbing()
        {
            if (IsClimbing)
            {
                if (_climbContactCount > 1)
                {
                    _climbNormal.Normalize();
                    float upDot = Vector3.Dot(_upAxis, _climbNormal);
                    if (upDot >= _minGroundDotProduct)
                    {
                        _climbNormal = _lastClimbNormal;
                    }
                }

                _groundContactCount = 1;
                _contactNormal = _climbNormal;
                Debug.Log("Climb check returned true");
                return true;
            }

            return false;
        }

        public bool SnapToGround()
        {
            if (_stepsSinceLastGrounded > 1 || _stepsSinceLastJump <= 2 || InWater)
                return false;

            float speed = _velocity.magnitude;
            if (speed > _maxSnapSpeed)
                return false;

            float offset = 0.05f;
            Vector3 capsuleCenter = _rigidbody.position + _collider.center;
            float capsuleHeight = _collider.height;
            float capsuleRadius = _collider.radius - offset;

            Vector3 point1 = capsuleCenter + _upAxis * (-capsuleHeight * 0.5f + capsuleRadius);
            Vector3 point2 = capsuleCenter + _upAxis * (capsuleHeight * 0.5f - capsuleRadius);

            if (!Physics.CapsuleCast(point1, point2, capsuleRadius, -_upAxis,
                    out RaycastHit hit, _probeDistance, _probeMask, QueryTriggerInteraction.Ignore))
            {
                Debug.DrawRay(_position + _upAxis * _collider.center.y - _upAxis * (_collider.radius + 0.1f), _rightAxis * _probeDistance, Color.black, 10f);
                Debug.DrawRay(_position - _upAxis * _collider.radius + _upAxis * _collider.height, _rightAxis * _probeDistance, Color.yellow, 10f);
                Debug.DrawLine(_position, _position + -_upAxis * _probeDistance, Color.red, 10f);
                Debug.DrawLine(_position, _position + _upAxis * _collider.height, Color.magenta, 10f);
                return false;
            }

            Debug.Log("Collider that was hit: " + hit.collider.name);

            Debug.DrawRay(_position + _upAxis * _collider.center.y - _upAxis * (_collider.radius), _rightAxis * _probeDistance, Color.yellow, 10f);
            Debug.DrawRay(_position - _upAxis * _collider.radius + _upAxis * _collider.height, _rightAxis * _probeDistance, Color.yellow, 10f);
            Debug.DrawLine(_position, _position + _upAxis * _collider.height, Color.magenta, 10f);
            Debug.DrawLine(_position, hit.point, Color.green, 10f);

            float upDot = Vector3.Dot(_upAxis, hit.normal);
            Debug.LogWarning("UpDot: " + upDot + " MinGroundDotProduct: " + _minGroundDotProduct + " hit.normal " + hit.normal +" contact normal : " + _contactNormal);
            if (upDot < _minGroundDotProduct)
            {
                return false;
            }

            Debug.Log("SnapToGround returned");
            _isSnapping = true;
            _groundContactCount = 1;
            _snapContactNormal = hit.normal;
            _distanceFromGround = hit.distance;
            _connectedRigidbody = hit.rigidbody;
            return true;
        }

        public bool CheckSteepContacts()
        {
            if (_steepContactCount > 1)
            {
                _steepNormal.Normalize();
                float upDot = Vector3.Dot(_upAxis, _steepNormal);
                if (upDot >= _minGroundDotProduct && upDot <= _minSteepDotProduct)
                {
                    _groundContactCount = 1;
                    _steepContactCount = 0;
                    _contactNormal = _steepNormal;
                    Debug.Log("Steep contacts check returned true");
                    return true;
                }
            }

            return false;
        }

        public void ClearState()
        {
            _isSnapping = false;
            _submergence = 0f;
            _groundContactCount = _steepContactCount = _climbContactCount = 0;
            _contactNormal = _steepNormal = _climbNormal = Vector3.zero;
            _connectionVelocity = Vector3.zero;
            _previousConnectedRigidbody = _connectedRigidbody;
            _connectedRigidbody = null;
        }

        #endregion

        #region Booleans

        public bool HaveWeWon()
        {
            return _haveWeWon;
        }

        public bool HaveWeLost()
        {
            return _haveWeLost;
        }

        #endregion
        
        #region Updates


        void UpdateConnectionState()
        {
            if (_connectedRigidbody == _previousConnectedRigidbody)
            {
                Vector3 connectionMovement = _connectedRigidbody.transform.TransformPoint(_connectionLocalPosition) - _connectionWorldPosition;
                _connectionVelocity = connectionMovement / Time.deltaTime;
            }

            _connectionWorldPosition = _position;
            _connectionLocalPosition = _connectedRigidbody.transform.InverseTransformPoint(_connectionWorldPosition);
        }

        #endregion

        #region Rigidbody Methods

        public void SaveRigidBody()
        {
            _velocity = _rigidbody.velocity;
            _position = _rigidbody.position;
        }

        public void UpdateRigidBody()
        {
            _rigidbody.MovePosition(_position);
            _rigidbody.velocity = _velocity;
        }

        #endregion

        #region Setters and Getters

        public void SetMinClimbDotProduct(float minClimbDotProduct)
        {
            _minClimbDotProduct = minClimbDotProduct;
        }

        public void GetMinClimbDotProduct()
        {
            _minClimbDotProduct = Mathf.Cos(_maxClimbAngle * Mathf.Deg2Rad);
        }

        public void SetUpAxisTransform(Transform upAxisTransform)
        {
            _upAxisTransform = upAxisTransform;
        }

        public void SetTargetRotation(Quaternion targetRotation)
        {
            _targetRotation = targetRotation;
        }

        public Quaternion GetTargetRotation()
        {
            return _targetRotation;
        }

        public void SetUpAxis(Vector3 upAxis)
        {
            _upAxis = upAxis;
        }

        public Vector3 GetUpAxis()
        {
            return _upAxis;
        }

        public void SetRightAxis(Vector3 rightAxis)
        {
            _rightAxis = rightAxis;
        }

        public Vector3 GetRightAxis()
        {
            return _rightAxis;
        }

        public void SetForwardAxis(Vector3 forwardAxis)
        {
            _forwardAxis = forwardAxis;
        }

        public Vector3 GetForwardAxis()
        {
            return _forwardAxis;
        }

        public float GetMinSteepDotProduct()
        {
            return _minSteepDotProduct;
        }

        public bool GetIsGrounded()
        {
            Debug.Log("IsGrounded: " + IsGrounded);
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

        public float GetNextJumpTime()
        {
            return _nextJumpTime;
        }

        public void SetNextJumpTime(float nextJumpTime)
        {
            _nextJumpTime = nextJumpTime;
        }

        public float GetNextDashTime()
        {
            return _nextDashTime;
        }

        public void SetNextDashTime(float nextDashTime)
        {
            _nextDashTime = nextDashTime;
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

        public void SetVelocity(Vector3 velocity)
        {
            _velocity = velocity;
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

        public Vector3 GetDesiredVelocity()
        {
            return _desiredVelocity;
        }

        public void SetDesiredVelocity(Vector3 desiredVelocity)
        {
            _desiredVelocity = desiredVelocity;
        }

        public bool GetIntentToJump()
        {
            return _intentToJump;
        }

        public void SetIntentToJump(bool intentToJump)
        {
            _intentToJump = intentToJump;
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

        #endregion

        #region Math Methods

        public Vector3 ProjectDirectionOnContactPlane(Vector3 direction, Vector3 normal)
        {
            return (direction - normal * Vector3.Dot(direction, normal)).normalized;
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

        public void CalculateMinClimbDotProduct()
        {
            _minClimbDotProduct = Mathf.Cos(_maxClimbAngle * Mathf.Deg2Rad);
        }

        #endregion

        #region Collision Methods


        public void EvaluateCollisions(Collision collision)
        {
            int layer = collision.gameObject.layer;
            Debug.Log("Layer: " + layer);
            if ((_probeMask & (1 << layer)) != 0)
            {
                Debug.Log("Probe Mask");
                for (int i = 0; i < collision.contactCount; i++)
                {
                    Debug.Log("Contact count: " + collision.contactCount);
                    Vector3 normal = collision.GetContact(i).normal;
                    float upDot = Vector3.Dot(_upAxis, normal);
                    //check ground contacts
                    if (upDot >= _minGroundDotProduct)
                    {
                        Debug.Log("Ground Contact");
                        _groundContactCount += 1;
                        _contactNormal += normal;
                        _connectedRigidbody = collision.rigidbody;
                        continue;
                    }

                    //if not ground contact is it steep contact?
                    if (upDot < _minGroundDotProduct && upDot >= _minSteepDotProduct)
                    {
                        Debug.Log("Steep Contact");
                        _steepContactCount += 1;
                        _steepNormal += normal;
                        if (_groundContactCount == 0)
                        {
                            _connectedRigidbody = collision.rigidbody;
                        }

                        continue;
                    }

                    //if not steep contact is it climb contact?
                    if (upDot < _minSteepDotProduct && upDot >= _minClimbDotProduct)
                    {
                        Debug.Log("Climb Contact");
                        _climbContactCount += 1;
                        _climbNormal += normal;
                        _lastClimbNormal = normal;
                        _connectedRigidbody = collision.rigidbody;
                    }
                }

                return;
            }

            if ((_stairMask & (1 << layer)) != 0)
            {
                for (int i = 0; i < collision.contactCount; i++)
                {
                    Vector3 normal = collision.GetContact(i).normal;
                    float upDot = Vector3.Dot(_upAxis, normal);
                    if (upDot >= _minStairsDotProduct)
                    {
                        _groundContactCount += 1;
                        _contactNormal += normal;
                        _connectedRigidbody = collision.rigidbody;
                        continue;
                    }

                    if (upDot < _minStairsDotProduct && upDot >= _minSteepDotProduct)
                    {
                        _steepContactCount += 1;
                        _steepNormal += normal;
                        if (_groundContactCount == 0)
                        {
                            _connectedRigidbody = collision.rigidbody;
                        }

                        continue;
                    }

                    if (upDot >= _minClimbDotProduct)
                    {
                        _climbContactCount += 1;
                        _climbNormal += normal;
                        _lastClimbNormal = normal;
                        _connectedRigidbody = collision.rigidbody;
                    }
                }

                return;
            }

            if ((_climbMask & (1 << layer)) != 0)
            {
                for (int i = 0; i < collision.contactCount; i++)
                {
                    Vector3 normal = collision.GetContact(i).normal;
                    float upDot = Vector3.Dot(_upAxis, normal);

                    if (upDot >= _minGroundDotProduct)
                    {
                        _groundContactCount += 1;
                        _contactNormal += normal;
                        _connectedRigidbody = collision.rigidbody;
                        continue;
                    }

                    if (upDot < _minGroundDotProduct && upDot >= _minSteepDotProduct)
                    {
                        _steepContactCount += 1;
                        _steepNormal += normal;
                        if (_groundContactCount == 0)
                        {
                            _connectedRigidbody = collision.rigidbody;
                        }
                    }
                }
            }
        }


        #endregion

        #region Trigger Methods

        public void HandleTriggerEnter(Collider collider)
        {
            int layer = collider.gameObject.layer;
            if ((_waterMask & (1 << layer)) != 0)
            {
                EvaluateSubmergence(collider);
            }
        }


        public void HandleTriggerStay(Collider collider)
        {
            int layer = collider.gameObject.layer;
            if ((_waterMask & (1 << layer)) != 0)
            {
                EvaluateSubmergence(collider);
            }
        }

        private void EvaluateSubmergence(Collider collider)
        {
            if (Physics.Raycast(_rigidbody.position + _upAxis * _submergenceOffset, -_upAxis, out RaycastHit hit, _submergenceRange + 1, _waterMask, QueryTriggerInteraction.Collide))
            {
                _submergence = 1f - hit.distance / _submergenceRange;
            }
            else
            {
                _submergence = 0f;
            }
        }
        #endregion
        
        public bool IsMovingUp()
        {
            Debug.Log("Dot: " + Vector3.Dot(_velocity, _upAxis));
            return Vector3.Dot(_velocity, _upAxis) >= -0.5f;
        }
    }
}