using System;
using UnityEngine;

namespace FGJ24.Player
{
    [Serializable]
    public class CharacterCraftStats
    {
        [SerializeField] private float _craftSpeed;
        [SerializeField] private float _craftAcceleration;
        [SerializeField] private float _craftTurnSpeed;
        
        [SerializeField] private float _craftDuration;
        [SerializeField] private float _craftCooldown;
        
        public float GetCraftSpeed()
        {
            return _craftSpeed;
        }
        
        public float GetCraftAcceleration()
        {
            return _craftAcceleration;
        }
        
        public float GetCraftTurnSpeed()
        {
            return _craftTurnSpeed;
        }
        
        public float GetCraftDuration()
        {
            return _craftDuration;
        }
        
        public float GetCraftCooldown()
        {
            return _craftCooldown;
        }
    }
}