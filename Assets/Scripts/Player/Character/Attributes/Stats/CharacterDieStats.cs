using System;
using UnityEngine;

namespace FGJ24.Player
{
    [Serializable]
    public class CharacterDieStats
    {
        [SerializeField] private float _dieSpeed;
        [SerializeField] private float _dieAcceleration;
        [SerializeField] private float _dieTurnSpeed;
        
        [SerializeField] private float _dieDuration;
        [SerializeField] private float _dieCooldown;
        
        public float GetDieSpeed()
        {
            return _dieSpeed;
        }
        
        public float GetDieAcceleration()
        {
            return _dieAcceleration;
        }
        
        public float GetDieTurnSpeed()
        {
            return _dieTurnSpeed;
        }
        
        public float GetDieDuration()
        {
            return _dieDuration;
        }
        
        public float GetDieCooldown()
        {
            return _dieCooldown;
        }
    }
}