using System;
using UnityEngine;

namespace FGJ24.Player
{
    [Serializable]
    public class CharacterWinStats
    {
        [SerializeField] private float _winSpeed;
        [SerializeField] private float _winAcceleration;
        [SerializeField] private float _winTurnSpeed;
        
        [SerializeField] private float _winDuration;
        [SerializeField] private float _winCooldown;
        
        public float GetWinSpeed()
        {
            return _winSpeed;
        }
        
        public float GetWinAcceleration()
        {
            return _winAcceleration;
        }
        
        public float GetWinTurnSpeed()
        {
            return _winTurnSpeed;
        }
        
        public float GetWinDuration()
        {
            return _winDuration;
        }
        
        public float GetWinCooldown()
        {
            return _winCooldown;
        }
        
    }
}