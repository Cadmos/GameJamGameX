using System;
using UnityEngine;

namespace FGJ24.Player
{
    [Serializable]
    public class CharacterLandingStats
    {
        [SerializeField] private float _landingSpeed;
        [SerializeField] private float _landingAcceleration;

        [SerializeField] private float _landingDuration;
        
        public float GetLandingSpeed()
        {
            return _landingSpeed;
        }
        
        public float GetLandingAcceleration()
        {
            return _landingAcceleration;
        }
        
        public float GetLandingDuration()
        {
            return _landingDuration;
        }
    }
}