using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace FGJ24.Player
{
    [Serializable]
    public class CharacterDashStats
    {
        [SerializeField] private float _dashSpeed = 20f;
        [SerializeField] private float _dashMaxSpeed = 25f;
        
        [SerializeField] private float _initialDashAcceleration = 1000f;
        [SerializeField] private float _dashAcceleration = 5f;
        
        [SerializeField] private float _dashTurnSpeed = 10f;
        
        [SerializeField] private float _dashDuration = 0.2f;
        [SerializeField] private float _dashCooldownDuration = 1f;
        
        [SerializeField] private AnimationCurve _dashCurve;
        [SerializeField] private float _dashCurveDuration = 0.5f;
        
        public float GetDashSpeed()
        {
            return _dashSpeed;
        }
        
        public float GetInitialDashAcceleration()
        {
            return _initialDashAcceleration;
        }
        public float GetDashAcceleration()
        {
            return _dashAcceleration;
        }
        
        public float GetDashTurnSpeed()
        {
            return _dashTurnSpeed;
        }
        public float GetDashDuration()
        {
            return _dashDuration;
        }
        public float GetDashCooldownDuration()
        {
            return _dashCooldownDuration;
        }
        
        
        public AnimationCurve GetDashCurve()
        {
            return _dashCurve;
        }
        
        public float GetDashCurveDuration()
        {
            return _dashCurveDuration;
        }


        public float GetDashMaxSpeed()
        {
            return _dashMaxSpeed;
        }
    }
}