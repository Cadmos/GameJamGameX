using System;
using UnityEngine;

namespace FGJ24.Player
{
    [Serializable]
    public class CharacterFallingStats
    {
        [SerializeField] private float _fallingSpeed;
        [SerializeField] private float _maxFallingSpeed;
        [SerializeField] private float _fallingAcceleration;
        [SerializeField] private float _fallingTurnSpeed;

        [SerializeField] private float _fallingDuration;
        [SerializeField] private float _fallingCooldown;

        public float GetFallingSpeed()
        {
            return _fallingSpeed;
        }
        
        public float GetMaxFallingSpeed()
        {
            return _maxFallingSpeed;
        }

        public float GetFallingAcceleration()
        {
            return _fallingAcceleration;
        }
        
        public float GetFallingTurnSpeed()
        {
            return _fallingTurnSpeed;
        }

        public float GetFallingDuration()
        {
            return _fallingDuration;
        }
        
        public float GetFallingCooldown()
        {
            return _fallingCooldown;
        }

    }
}