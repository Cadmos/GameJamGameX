using System;
using UnityEngine;

namespace FGJ24.Player
{
    [Serializable]
    public class CharacterMoveSpeed
    {
        [SerializeField] private float _moveSpeed;
        
        public float GetMoveSpeed()
        {
            return _moveSpeed;
        }
    }
}