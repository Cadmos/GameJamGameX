using System;
using UnityEngine;

namespace FGJ24.Player
{
    [Serializable]
    public class CharacterJump
    {
        [SerializeField] private float _jumpForce = 10f;
        [SerializeField] private float _movementDampening = 0.05f;
        [SerializeField] private float _jumpCooldownDurationAfterLanding = 0.1f;
        [SerializeField] private float _controllableAirDuration = 5f;
        
        [SerializeField] private AnimationCurve _jumpCurve;
        [SerializeField] private float _jumpCurveDuration = 3f;
        
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
    }
}