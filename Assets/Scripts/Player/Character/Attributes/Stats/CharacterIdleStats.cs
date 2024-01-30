using System;
using UnityEngine;

namespace FGJ24.Player
{
    [Serializable]
    public class CharacterIdleStats
    {
        [SerializeField] private float _idleSpeed;
        [SerializeField] private float _idleAcceleration;
        [SerializeField] private float _idleTurningSpeed;
        
        [SerializeField] private float _idleDuration;
        [SerializeField] private float _idleCooldown;
        
        public float GetIdleSpeed()
        {
            return _idleSpeed;
        }
        
        public float GetIdleAcceleration()
        {
            return _idleAcceleration;
        }
        
        public float GetIdleTurningSpeed()
        {
            return _idleTurningSpeed;
        }
        
        public float GetIdleDuration()
        {
            return _idleDuration;
        }
        
        public float GetIdleCooldown()
        {
            return _idleCooldown;
        }
    }
}