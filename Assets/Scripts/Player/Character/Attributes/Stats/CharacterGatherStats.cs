using System;
using UnityEngine;

namespace FGJ24.Player
{
    [Serializable]
    public class CharacterGatherStats
    {
        [SerializeField] private float _gatherSpeed;
        [SerializeField] private float _gatherAcceleration;
        [SerializeField] private float _gatherTurnSpeed;
        
        [SerializeField] private float _gatherDuration;
        [SerializeField] private float _gatherCooldown;
        
        public float GetGatherSpeed()
        {
            return _gatherSpeed;
        }
        
        public float GetGatherAcceleration()
        {
            return _gatherAcceleration;
        }
        
        public float GetGatherTurnSpeed()
        {
            return _gatherTurnSpeed;
        }
        
        public float GetGatherDuration()
        {
            return _gatherDuration;
        }
        
        public float GetGatherCooldown()
        {
            return _gatherCooldown;
        }
    }
}