using System;
using UnityEngine;

namespace FGJ24.Player
{
    [Serializable]
    public class CharacterMineStats
    {
        [SerializeField] private float _mineSpeed;
        [SerializeField] private float _mineAcceleration;
        [SerializeField] private float _mineTurnSpeed;
        
        [SerializeField] private float _mineDuration;
        [SerializeField] private float _mineCooldown;
        
        public float GetMineSpeed()
        {
            return _mineSpeed;
        }
        
        public float GetMineAcceleration()
        {
            return _mineAcceleration;
        }
        
        public float GetMineTurnSpeed()
        {
            return _mineTurnSpeed;
        }
        
        public float GetMineDuration()
        {
            return _mineDuration;
        }
        
        public float GetMineCooldown()
        {
            return _mineCooldown;
        }
    }
}