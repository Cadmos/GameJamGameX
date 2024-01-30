using System;
using UnityEngine;

namespace FGJ24.Player
{
    [Serializable]
    public class CharacterStoppingStats
    {
        [SerializeField] private float _stoppingSpeed;
        [SerializeField] private float _stoppingAcceleration;
        [SerializeField] private float _stoppingTurnSpeed;

        [SerializeField] private float _stoppingDuration;
        [SerializeField] private float _stoppingCooldown;

        public float GetStoppingSpeed()
        {
            return _stoppingSpeed;
        }

        public float GetStoppingAcceleration()
        {
            return _stoppingAcceleration;
        }
        
        public float GetStoppingTurnSpeed()
        {
            return _stoppingTurnSpeed;
        }

        public float GetStoppingDuration()
        {
            return _stoppingDuration;
        }
        
        public float GetStoppingCooldown()
        {
            return _stoppingCooldown;
        }
    }
}