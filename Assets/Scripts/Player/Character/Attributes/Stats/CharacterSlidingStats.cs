using System;
using UnityEngine;

namespace FGJ24.Player
{
    [Serializable]
    public class CharacterSlidingStats
    {
        [SerializeField] private float _slidingSpeed;
        [SerializeField] private float _maxSlidingSpeed;
        [SerializeField] private float _slidingAcceleration;
        [SerializeField] private float _slidingTurnSpeed;

        public float GetSlidingSpeed()
        {
            return _slidingSpeed;
        }
        
        public float GetMaxSlidingSpeed()
        {
            return _maxSlidingSpeed;
        }
        public float GetSlidingAcceleration()
        {
            return _slidingAcceleration;
        }
        
        public float GetSlidingTurnSpeed()
        {
            return _slidingTurnSpeed;
        }
        
    }
}