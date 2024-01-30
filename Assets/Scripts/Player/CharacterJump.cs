using System;
using UnityEngine;

namespace FGJ24.Player
{
    [Serializable]
    public class CharacterJumpStats
    {
        [SerializeField] private float _jumpHeight = 5f;
        [SerializeField] private float _jumpSpeed = 20f;
        [SerializeField] private float _jumpAcceleration = 10f;
        
        [SerializeField] private float _jumpCooldownAfterJumping = 0.2f;
        
        [SerializeField] private float _jumpTurnSpeed = 10f;
        [SerializeField] private float _fallSpeedThreshold = 10f;
        
        
        
        [SerializeField] private float _jumpForce = 10f;
        [SerializeField] private float _movementDampening = 0.05f;
        [SerializeField] private float _jumpCooldownDurationAfterLanding = 0.1f;
        [SerializeField] private float _controllableAirDuration = 5f;
        
        [SerializeField] private AnimationCurve _jumpCurve;
        [SerializeField] private float _jumpCurveDuration = 3f;
        
        public float GetJumpHeight()
        {
            return _jumpHeight;
        }
        public float GetJumpSpeed()
        {
            return _jumpSpeed;
        }
        public float GetJumpAcceleration()
        {
            return _jumpAcceleration;
        }
        public float GetJumpCooldownAfterJumping()
        {
            return _jumpCooldownAfterJumping;
        }
        public float GetJumpTurnSpeed()
        {
            return _jumpTurnSpeed;
        }
        
        public float GetJumpForce()
        {
            return _jumpForce;
        }
        
        public float GetMovementDampening()
        {
            return _movementDampening;
        }
        
        public float GetJumpCooldownDurationAfterLanding()
        {
            return _jumpCooldownDurationAfterLanding;
        }
        
        public float GetControllableAirDuration()
        {
            return _controllableAirDuration;
        }
        
        public AnimationCurve GetJumpCurve()
        {
            return _jumpCurve;
        }
        
        public float GetJumpCurveDuration()
        {
            return _jumpCurveDuration;
        }

        public float FallSpeedThreshold()
        {
            return _fallSpeedThreshold;
        }
    }
}