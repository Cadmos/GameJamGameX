using System;
using UnityEngine;

namespace FGJ24.Player
{
    [Serializable]
    public class CharacterDash
    {
        [SerializeField] private float dashSpeed = 20f;
        [SerializeField] private float dashDuration = 0.2f;
        [SerializeField] private float dashCooldownDuration = 1f;
        [SerializeField] private AnimationCurve dashCurve;
        [SerializeField] private float dashCurveDuration = 0.5f;
        [SerializeField] private float _dashAcceleration = 0.5f;
        
        
        public float GetDashSpeed()
        {
            return dashSpeed;
        }
        
        public float GetDashDuration()
        {
            return dashDuration;
        }
        
        public float GetDashCooldownDuration()
        {
            return dashCooldownDuration;
        }
        
        public AnimationCurve GetDashCurve()
        {
            return dashCurve;
        }
        
        public float GetDashCurveDuration()
        {
            return dashCurveDuration;
        }

        public float GetDashAcceleration()
        {
            return _dashAcceleration;
        }
    }
}