using System;
using FGJ24.CustomPhysics;
using UnityEngine;
using UnityEngine.Serialization;

namespace FGJ24.Player
{
    [Serializable]
    public class PlayerController
    {
        #region Properties

        [SerializeField] private Rigidbody _rigidbody, _connectedRigidbody, _previousConnectedRigidbody;
        
        
        [SerializeField] private Transform _cameraTransform;
        [SerializeField] private Transform _playerInputSpace = default;
        [SerializeField] private CapsuleCollider _collider;
        [SerializeField] private Transform _upAxisTransform;
        [SerializeField] private Quaternion _gravityAlignment = Quaternion.identity;
        [SerializeField] private float _upAlignmentSpeed = 360f;
        [SerializeField] private Vector3 _gravity;

        [SerializeField] private Vector3 _upAxis, _rightAxis, _forwardAxis;
        [SerializeField] private Quaternion _targetRotation;

        [SerializeField] private bool _haveWeWon;
        [SerializeField] private bool _haveWeLost;

        [SerializeField] private float _maxGroundAngle = 45f;

        [SerializeField] private LayerMask _stairMask = -1;
        [SerializeField] private LayerMask _probeMask = -1;

        [SerializeField] private float _maxStairsAngle = 75f;
        [SerializeField] private float _maxSteepAngle = 75f;

        [SerializeField] private float _maxSnapSpeed = 100f;
        [SerializeField] private float _probeDistance = 1f;
        [SerializeField] private Vector3 _velocity, _desiredVelocity, _connectionVelocity;
        [SerializeField] private Vector3 _position, _connectionWorldPosition, _connectionLocalPosition;

        public bool WasGroundedLastFrame => _stepsSinceLastGrounded <= 1;
        public bool IsGrounded => _groundContactCount > 0;

        public bool IsSnapping => _isSnapping;
        public bool IsSteep => _steepContactCount > 0;

        [SerializeField] private bool _isSnapping;
        [SerializeField] private int _groundContactCount;
        [SerializeField] private int _steepContactCount;
        [SerializeField] private float _minGroundDotProduct;
        [SerializeField] private float _minStairsDotProduct;
        [SerializeField] private float _minSteepDotProduct;

        [SerializeField] private Vector3 _contactNormal;
        [SerializeField] private Vector3 _steepNormal;

        [SerializeField] private int _stepsSinceLastGrounded;
        [SerializeField] private int _stepsSinceLastJump;

        [SerializeField] private int _jumpPhase;
        [SerializeField] private bool _intentToJump;
        [SerializeField] private bool _intentToDash;
        [SerializeField] private int _maxAirJumps;
        [SerializeField] private float _nextDashTime;
        [SerializeField] private float _dashStartTime;
        [SerializeField] private float _landingTime;
        [SerializeField] private float _nextJumpTime;
        [SerializeField] private float _distanceFromGround;
        [SerializeField] private float _stepSmooth = 0.2f;
        [SerializeField] private Vector3 _snapContactNormal;


        #endregion

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

        void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(_position + _collider.center - _upAxis * (_collider.radius), _collider.radius);
            Gizmos.DrawWireSphere(_position - _upAxis * _collider.radius + _upAxis * _collider.height, _collider.radius);
            Debug.DrawLine(_position, _position + -_upAxis * _probeDistance, Color.red, 10f);
            Debug.DrawLine(_position, _position + _upAxis * _collider.height, Color.magenta, 10f);
        }

        public void Falling(Vector3 velocity, float horizontalAcceleration, float verticalAcceleration, Vector3 desiredVelocity)
        {
            _stepsSinceLastGrounded += 1;
            _stepsSinceLastJump += 1;

            if (GetIsGrounded() || CheckSteepContacts())
            {
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
                _contactNormal = _upAxis;
            }

            velocity = HorizontalMovement(velocity, horizontalAcceleration, desiredVelocity, _contactNormal);
            //velocity = VerticalMovement(velocity, verticalAcceleration, desiredVelocity.y, _contactNormal);

            velocity += _gravity * Time.deltaTime;
            _velocity = velocity;
        }


        public void AlignCollider()
        {
            _collider.transform.rotation = Quaternion.LookRotation(_forwardAxis, _upAxis);
        }

        public void Move(Vector3 velocity, float acceleration, Vector3 desiredVelocity)
        {
            _stepsSinceLastGrounded += 1;
            _stepsSinceLastJump += 1;

            //are we on an actionable surface?
            if (GetIsGrounded() || SnapToGround() || CheckSteepContacts())
            {
                if (_isSnapping)
                {
                    _contactNormal = _snapContactNormal != Vector3.zero ? _snapContactNormal : _upAxis;
                    float dot = Vector3.Dot(velocity, _contactNormal);
                    if (dot > 0f)
                    {
                        velocity = (velocity - _contactNormal * dot).normalized * velocity.magnitude;
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
                _contactNormal = _upAxis;
            }
            
            if (_connectedRigidbody) {
                if (_connectedRigidbody.isKinematic || _connectedRigidbody.mass >= _rigidbody.mass) {
                    UpdateConnectionState();
                }
            }

            velocity = HorizontalMovement(velocity, acceleration, desiredVelocity, _contactNormal);

            if (_isSnapping)
            {
                if (_distanceFromGround > _stepSmooth)
                {
                    _position -= _upAxis * _stepSmooth;
                }
            }
            


            AdjustVelocityDebug(velocity, desiredVelocity);
            velocity += _gravity * Time.deltaTime;
            _velocity = velocity;
        }

        public void Sliding(Vector3 velocity, float acceleration, Vector3 desiredVelocity)
        {
            _stepsSinceLastGrounded += 1;
            _stepsSinceLastJump += 1;

            if (GetIsGrounded() || CheckSteepContacts())
            {
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
                _contactNormal = _upAxis;
            }

            velocity = HorizontalMovement(velocity, acceleration, desiredVelocity, _contactNormal);
            //velocity = VerticalMovement(velocity, acceleration, desiredVelocity.y, _contactNormal);

            AdjustVelocityDebug(velocity, desiredVelocity);
            velocity += _gravity * Time.deltaTime;
            _velocity = velocity;
        }

        public void Jump(Vector3 velocity, float acceleration, Vector3 desiredVelocity, Vector3 contactNormal)
        {
            _stepsSinceLastGrounded += 1;
            _stepsSinceLastJump += 1;

            if (GetIsGrounded() || SnapToGround() || CheckSteepContacts())
            {
                _stepsSinceLastGrounded = 0;
                if (_stepsSinceLastJump > 1)
                {
                    _jumpPhase = 0;
                }

                if (_groundContactCount > 1)
                    contactNormal.Normalize();
            }
            else
            {
                contactNormal = _upAxis;
            }

            HorizontalMovement(velocity, acceleration, desiredVelocity, contactNormal);
        }

        public void JumpToHeight(Vector3 velocity, Vector3 contactNormal, float jumpHeight)
        {
            if (_intentToJump)
            {
                _intentToJump = false;
                Vector3 jumpDirection;
                if (IsGrounded)
                {
                    jumpDirection = contactNormal;
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
                float jumpSpeed = Mathf.Sqrt(-2f * Physics.gravity.magnitude * jumpHeight);
                jumpDirection = (jumpDirection + _upAxis).normalized;
                float alignedSpeed = Vector3.Dot(velocity, jumpDirection);

                if (alignedSpeed > 0f)
                {
                    jumpSpeed = Mathf.Max(jumpSpeed - alignedSpeed, 0f);
                }

                _velocity += _upAxis * jumpSpeed;
            }
        }

        public void EvaluateCollisions(Collision collision)
        {
            float minDot = GetMinDot(collision.gameObject.layer);
            for (int i = 0; i < collision.contactCount; i++)
            {
                Vector3 normal = collision.GetContact(i).normal;
                float upDot = Vector3.Dot(_upAxis, normal);
                if (upDot >= minDot)
                {
                    _groundContactCount += 1;
                    _contactNormal += normal;
                    _connectedRigidbody = collision.rigidbody;
                }
                else if (upDot > -0.01f)
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

        public bool SnapToGround()
        {
            if (_stepsSinceLastGrounded > 1 || _stepsSinceLastJump <= 2)
                return false;

            float speed = _velocity.magnitude;
            if (speed > _maxSnapSpeed)
                return false;

            if (!Physics.CapsuleCast(_position + _upAxis * _collider.center.y - _upAxis * (_collider.radius),
                    _position - _upAxis * _collider.radius + _upAxis * _collider.height,
                    _collider.radius,
                    -_upAxis,
                    out RaycastHit hit, _probeDistance, _probeMask))
            {
                Debug.DrawRay(_position + _upAxis * _collider.center.y - _upAxis * (_collider.radius), _rightAxis * _probeDistance, Color.yellow, 10f);
                Debug.DrawRay(_position - _upAxis * _collider.radius + _upAxis * _collider.height, _rightAxis * _probeDistance, Color.yellow, 10f);
                Debug.DrawLine(_position, _position + -_upAxis * _probeDistance, Color.red, 10f);
                Debug.DrawLine(_position, _position + _upAxis * _collider.height, Color.magenta, 10f);
                return false;
            }

            Debug.DrawRay(_position + _upAxis * _collider.center.y - _upAxis * (_collider.radius), _rightAxis * _probeDistance, Color.yellow, 10f);
            Debug.DrawRay(_position - _upAxis * _collider.radius + _upAxis * _collider.height, _rightAxis * _probeDistance, Color.yellow, 10f);
            Debug.DrawLine(_position, _position + _upAxis * _collider.height, Color.magenta, 10f);
            Debug.DrawLine(_position, hit.point, Color.green, 10f);


            if (GroundCheck(_upAxis, hit.normal, hit.collider.gameObject.layer))
                return false;

            _isSnapping = true;
            _groundContactCount = 1;
            _snapContactNormal = hit.normal;
            _distanceFromGround = hit.distance;
            _connectedRigidbody = hit.rigidbody;
            return true;
        }

        private bool SteepCheck(float normalY, int gameObjectLayer)
        {
            if (normalY > GetMinDot(gameObjectLayer) && normalY < GetMinSteepDotProduct())
            {
                Debug.Log("SteepCheck True");
                return true;
            }

            Debug.Log("SteepCheck False");
            return false;
        }

        public bool GroundCheck(Vector3 upAxis, Vector3 hitNormal, int layer)
        {
            float upDot = Vector3.Dot(upAxis, hitNormal);
            if (upDot < GetMinDot(layer))
            {
                return true;
            }

            return false;
        }

        public void AdjustVelocity(Vector3 velocity, float acceleration, Vector3 desiredVelocity, bool enableSnap)
        {
            Debug.Log("Desired velocity: " + desiredVelocity + " velocity: " + velocity + " acceleration: " + acceleration + " enableSnap: " + enableSnap);
            if (_contactNormal == Vector3.zero)
            {
                Debug.Log("Contact normal is zero!");
                _contactNormal = desiredVelocity.normalized;
            }

            Vector3 xAxis = ProjectDirectionOnContactPlane(Vector3.right, _contactNormal).normalized;
            Vector3 zAxis = ProjectDirectionOnContactPlane(Vector3.forward, _contactNormal).normalized;

            float currentX = Vector3.Dot(velocity, xAxis);
            float currentZ = Vector3.Dot(velocity, zAxis);

            float maxSpeedChange = acceleration * Time.deltaTime;


            if (enableSnap)
            {
                desiredVelocity = ProjectDirectionOnContactPlane(desiredVelocity, _contactNormal);
            }

            float newX = Mathf.MoveTowards(currentX, desiredVelocity.x, maxSpeedChange);
            float newZ = Mathf.MoveTowards(currentZ, desiredVelocity.z, maxSpeedChange);


            Debug.Log(" currentY: " + velocity.y + " desiredVelocity.y: " + desiredVelocity.y + " maxSpeedChange: " + maxSpeedChange + " Time.deltaTime: " + Time.deltaTime + " acceleration: " + acceleration);

            if (!enableSnap)
            {
                float newY = Mathf.MoveTowards(velocity.y, desiredVelocity.y, maxSpeedChange);
                velocity.y = newY;
            }

            velocity += xAxis * (newX - currentX) + zAxis * (newZ - currentZ);
            _velocity = velocity;

            AdjustVelocityDebug(velocity, desiredVelocity);
        }

        private void AdjustVelocityDebug(Vector3 velocity, Vector3 desiredVelocity)
        {
            DebugTransform.Instance.SetForwardTransform(_upAxisTransform.forward);
            DebugTransform.Instance.SetRightTransform(_upAxisTransform.right);
            DebugTransform.Instance.SetUpTransform(_upAxisTransform.up);

            Debug.DrawLine(_rigidbody.position, _position + velocity, Color.green);
            Debug.DrawLine(_rigidbody.position, _position + desiredVelocity, Color.red);
        }

        public Vector3 HorizontalMovement(Vector3 velocity, float acceleration, Vector3 desiredVelocity, Vector3 contactNormal)
        {
            Vector3 xAxis = ProjectDirectionOnContactPlane(_rightAxis, contactNormal);
            Vector3 zAxis = ProjectDirectionOnContactPlane(_forwardAxis, contactNormal);

            Vector3 relativeVelocity = velocity - _connectionVelocity;
            float currentX = Vector3.Dot(relativeVelocity, xAxis);
            float currentZ = Vector3.Dot(relativeVelocity, zAxis);

            float maxSpeedChange = acceleration * Time.deltaTime;

            float newX = Mathf.MoveTowards(currentX, desiredVelocity.x, maxSpeedChange);
            float newZ = Mathf.MoveTowards(currentZ, desiredVelocity.z, maxSpeedChange);

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

        public Vector3 ProjectDirectionOnContactPlane(Vector3 direction, Vector3 normal)
        {
            return (direction - normal * Vector3.Dot(direction, normal)).normalized;
        }

        public void LimitVelocity(float maxSpeed)
        {
            float speed = _velocity.magnitude;
            if (speed > maxSpeed)
            {
                _velocity *= maxSpeed / (speed + Mathf.Epsilon);
            }
        }

        public void ClearState()
        {
            _isSnapping = false;
            _groundContactCount = _steepContactCount = 0;
            _contactNormal = _steepNormal = _connectionVelocity = Vector3.zero;
            _previousConnectedRigidbody = _connectedRigidbody;
            _connectedRigidbody = null;
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
                    return true;
                }
            }

            return false;
        }

        public void UpdateDesiredVelocity(Vector3 direction, float speed)
        {
            _desiredVelocity = new Vector3(direction.x, 0, direction.z) * speed;
        }

        public void UpdateGravityAlignment()
        {
            Vector3 fromUp = _gravityAlignment * Vector3.up;
            Vector3 toUp = CustomGravity.GetUpAxis(_position);
            float dot = Mathf.Clamp(Vector3.Dot(fromUp, toUp), -1f, 1f);
            float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;
            float maxAngle = _upAlignmentSpeed * Time.deltaTime;
            Quaternion newAlignment = Quaternion.FromToRotation(fromUp, toUp) * _gravityAlignment;
            if (angle <= maxAngle)
            {
                _gravityAlignment = newAlignment;
            }
            else
            {
                _gravityAlignment = Quaternion.SlerpUnclamped(_gravityAlignment, newAlignment, maxAngle / angle);
            }

            _upAxisTransform.rotation = _gravityAlignment;
            _forwardAxis = ProjectDirectionOnContactPlane(_cameraTransform.forward, _upAxis);
            _rightAxis = Vector3.Cross(_upAxis, _forwardAxis);
        }

        public void UpdateDirections()
        {
            if (_playerInputSpace)
            {
                _rightAxis = ProjectDirectionOnContactPlane(_playerInputSpace.right, _upAxis);
                _forwardAxis = ProjectDirectionOnContactPlane(_playerInputSpace.forward, _upAxis);
            }

            _rightAxis = ProjectDirectionOnContactPlane(Vector3.right, _upAxis);
            _forwardAxis = ProjectDirectionOnContactPlane(Vector3.forward, _upAxis);
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

        public bool CheckLastFrameGrounded()
        {
            return _stepsSinceLastGrounded <= 1;
        }

        public bool CheckIfJustJumped()
        {
            return _stepsSinceLastJump <= 2;
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

        public void SetVelocity(Vector3 velocity)
        {
            _velocity = velocity;
        }

        public void UpdateGravity()
        {
            Vector3 upAxis;
            _gravity = CustomGravity.GetGravity(_position, out upAxis);
            _upAxis = upAxis;
        }

        public void UpdateUpAxisTransform()
        {
            _upAxisTransform.up = _upAxis;
        }
        
        void UpdateConnectionState () {
            if (_connectedRigidbody == _previousConnectedRigidbody) {
                Vector3 connectionMovement = _connectedRigidbody.transform.TransformPoint(_connectionLocalPosition) - _connectionWorldPosition;
                _connectionVelocity = connectionMovement / Time.deltaTime;
            }
            _connectionWorldPosition = _rigidbody.position;
            _connectionLocalPosition = _connectedRigidbody.transform.InverseTransformPoint(_connectionWorldPosition);
        }
    }
}