using System;
using UnityEngine;

namespace FGJ24.Player
{
    [Serializable]
    public class CharacterDash
    {
        [SerializeField] private float dashSpeed = 20f;
        [SerializeField] private float dashDuration = 0.2f;
        [SerializeField] private float dashCooldownDuration = 1f;
        
        public float GetDashSpeed()
        {
            return dashSpeed;
        }
        
        public float GetDashDuration()
        {
            return dashDuration;
        }
        
        public float GetDashCooldownDuration()
        {
            return dashCooldownDuration;
        }
    }
}