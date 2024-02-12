using System;
using UnityEngine;

namespace FGJ24.Player
{
    [Serializable]
    public class PlayerController
    {
        
        #region Properties
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
        
        [SerializeField] private Vector3 _desiredVelocity;

        [SerializeField] private int _jumpPhase;
        [SerializeField] private bool _intentToJump;
        [SerializeField] private bool _intentToDash;
        [SerializeField] private int _maxAirJumps;
        [SerializeField] private float _nextDashTime;
        [SerializeField] private float _dashStartTime;
        [SerializeField] private float _landingTime;
        [SerializeField] private float _nextJumpTime;
       #endregion
        
        
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

            if (!Physics.Raycast(_rigidbody.position+Vector3.up*0.5f, Vector3.down, out RaycastHit hit, _probeDistance, _probeMask))
            {

                return false;
            }
            
            if (hit.normal.y < GetMinDot(hit.collider.gameObject.layer))
            {
                return false;
            }

            _isSnapping = true;
            _groundContactCount = 1;
            _contactNormal = hit.normal;
            
            _velocity = AdjustVelocityForSurfaceContact(_velocity, hit.normal, speed, _rigidbody.position, hit.point);

            return true;
        }

        /// <summary>
        /// Adjusts the velocity of an object to stick to a surface along the surface normal.
        /// </summary>
        /// <param name="velocity">The current velocity of the object.</param>
        /// <param name="contactNormal">The normal vector at the point of contact with the surface.</param>
        /// <param name="speed">The speed of the object.</param>
        /// <param name="position">The current position of the object.</param>
        /// <param name="contactPoint">The point of contact with the surface.</param>
        /// <returns>The adjusted velocity vector.</returns>
        private Vector3 AdjustVelocityForSurfaceContact(Vector3 velocity, Vector3 contactNormal, float speed, Vector3 position, Vector3 contactPoint)
        {
            velocity = RemoveComponentOfVelocityInDirectionOfNormal(velocity, contactNormal, speed);
            velocity = MakeContactWithSurfaceThisFrame(position, contactPoint, velocity, contactNormal);
            return velocity;
        }

        /// <summary>
        /// Removes the component of the velocity in the direction of the normal vector.
        /// </summary>
        /// <param name="velocity">The current velocity of the object.</param>
        /// <param name="normal">The normal vector.</param>
        /// <param name="speed">The speed of the object.</param>
        /// <returns>The adjusted velocity vector.</returns>
        private Vector3 RemoveComponentOfVelocityInDirectionOfNormal(Vector3 velocity, Vector3 normal, float speed)
        {
            float dot = Vector3.Dot(velocity, normal);
            if (dot > 0f)
            {
                velocity = (velocity - normal * dot).normalized * speed;
            }

            return velocity;
        }

        /// <summary>
        /// Adjusts the velocity of the object to make contact with the surface in this frame.
        /// </summary>
        /// <param name="position">The current position of the object.</param>
        /// <param name="contactPoint">The point of contact with the surface.</param>
        /// <param name="velocity">The current velocity of the object.</param>
        /// <param name="contactNormal">The normal vector at the point of contact with the surface.</param>
        /// <returns>The adjusted velocity vector.</returns>
        private Vector3 MakeContactWithSurfaceThisFrame(Vector3 position, Vector3 contactPoint, Vector3 velocity, Vector3 contactNormal)
        {
            Vector3 toSurface = contactPoint - position;
            Vector3 requiredVelocity = toSurface / Time.fixedDeltaTime;

            Vector3 requiredVelocityPerpendicular = Vector3.Project(requiredVelocity, contactNormal);
            Vector3 currentVelocityPerpendicular = Vector3.Project(velocity, contactNormal);

            if (currentVelocityPerpendicular.magnitude < requiredVelocityPerpendicular.magnitude)
            {
                Vector3 requiredVelocityParallel = requiredVelocity - requiredVelocityPerpendicular;
                Vector3 currentVelocityParallel = velocity - currentVelocityPerpendicular;
                velocity = currentVelocityParallel + requiredVelocityPerpendicular;
            }

            return velocity;
        }

        public void AdjustVelocity(Vector3 velocity, float acceleration, Vector3 desiredVelocity, bool enableSnap)
        {
            Debug.Log("Desired velocity: " + desiredVelocity + " velocity: " + velocity + " acceleration: " + acceleration + " enableSnap: " + enableSnap);
            if(_contactNormal == Vector3.zero)
            {
                Debug.Log("Contact normal is zero!");
                _contactNormal = desiredVelocity.normalized;
            }
            Vector3 xAxis = ProjectOnContactPlane(Vector3.right, _contactNormal).normalized;
            Vector3 zAxis = ProjectOnContactPlane(Vector3.forward, _contactNormal).normalized;

            float currentX = Vector3.Dot(velocity, xAxis);
            float currentZ = Vector3.Dot(velocity, zAxis);

            float maxSpeedChange = acceleration * Time.fixedDeltaTime;


            if (enableSnap)
            {
                desiredVelocity = ProjectOnContactPlane(desiredVelocity, _contactNormal);
            }

            float newX = Mathf.MoveTowards(currentX, desiredVelocity.x, maxSpeedChange);
            float newZ = Mathf.MoveTowards(currentZ, desiredVelocity.z, maxSpeedChange);
            
            
            Debug.Log(" currentY: " + velocity.y + " desiredVelocity.y: " + desiredVelocity.y + " maxSpeedChange: " + maxSpeedChange + " Time.fixedDeltaTime: " + Time.fixedDeltaTime + " acceleration: " + acceleration);

            if (!enableSnap)
            {
                float newY = Mathf.MoveTowards(velocity.y, desiredVelocity.y, maxSpeedChange);
                velocity.y = newY;
            }
            velocity += xAxis * (newX - currentX) + zAxis * (newZ - currentZ);
            _velocity = velocity;
            
            AdjustVelocityDebug(xAxis, zAxis, velocity, desiredVelocity);
        }
        
        
        public void AdjustVelocity(Vector3 velocity, float acceleration, Vector3 desiredVelocity, bool enableSnap, bool enablePerch)
        {
            if(_contactNormal == Vector3.zero)
            {
                _contactNormal = desiredVelocity.normalized;
            }
            
            Vector3 xAxis = ProjectOnContactPlane(Vector3.right, _contactNormal).normalized;
            Vector3 zAxis = ProjectOnContactPlane(Vector3.forward, _contactNormal).normalized;

            float currentX = Vector3.Dot(velocity, xAxis);
            float currentZ = Vector3.Dot(velocity, zAxis);

            float maxSpeedChange = acceleration * Time.fixedDeltaTime;


            if (enableSnap)
            {
                desiredVelocity = ProjectOnContactPlane(desiredVelocity, _contactNormal);
            }
            else
            {
                float newY = Mathf.MoveTowards(velocity.y, desiredVelocity.y, maxSpeedChange);
                velocity.y = newY;
            }

            float newX = Mathf.MoveTowards(currentX, desiredVelocity.x, maxSpeedChange);
            float newZ = Mathf.MoveTowards(currentZ, desiredVelocity.z, maxSpeedChange);
            
            velocity += xAxis * (newX - currentX) + zAxis * (newZ - currentZ);
            _velocity = velocity;
            
            AdjustVelocityDebug(xAxis, zAxis, velocity, desiredVelocity);
        }

        private void AdjustVelocityDebug(Vector3 xAxis, Vector3 zAxis, Vector3 velocity, Vector3 desiredVelocity)
        {
            DebugTransform.Instance.SetForwardTransform(zAxis);
            DebugTransform.Instance.SetRightTransform(xAxis);

            Debug.DrawLine(_rigidbody.position, _rigidbody.position + velocity, Color.green);
            Debug.DrawLine(_rigidbody.position, _rigidbody.position + desiredVelocity, Color.red);
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
        
        public float GetNextDashTime()
        {
            return _nextDashTime;
        }
        public void SetNextDashTime(float nextDashTime)
        {
            _nextDashTime = nextDashTime;
        }
        

        public Vector3 ProjectOnContactPlane(Vector3 vector, Vector3 contactNormal)
        {
            return vector - contactNormal * Vector3.Dot(vector, contactNormal);
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
                    Debug.Log("Steep contact! normaly: " + normal.y + " _minSteepDotProduct: " + _minSteepDotProduct + " normal: " + normal);
                    _steepContactCount += 1;
                    _steepNormal += normal;
                }
            }
        }

        public void ClearState()
        {
            _isSnapping = false;
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
        public Vector3 GetDesiredVelocity()
        {
            return _desiredVelocity;
        }
        public void SetDesiredVelocity(Vector3 desiredVelocity)
        {
            _desiredVelocity = desiredVelocity;
        }
        public void UpdateDesiredVelocity(Vector3 direction, float speed)
        {
            Vector3 moveDirection = new Vector3(direction.x, 0, direction.z);
            moveDirection = _cameraTransform.forward * moveDirection.z + _cameraTransform.right * moveDirection.x;
            moveDirection *= speed;
            moveDirection.y = direction.y;
            Debug.Log($"moveDirection: {moveDirection} speed: {speed} direction: {direction}");
            _desiredVelocity = moveDirection;
        }

        public void UpdateDesiredVelocity(Vector3 direction, float speed, float fallSpeed)
        {
            Vector3 moveDirection = new Vector3(direction.x, 0, direction.z);
            moveDirection = _cameraTransform.forward * moveDirection.z + _cameraTransform.right * moveDirection.x;
            moveDirection *= speed;
            moveDirection.y = fallSpeed;
            _desiredVelocity = moveDirection;
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
    }
}