using System;
using Ioni;
using UnityEngine;

namespace FGJ24.Player
{
    [Serializable]
    public class CharacterMoveSpeed
    {
        [SerializeField] private float _maxMoveSpeed;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _minMoveSpeed;
        [SerializeField] private Player _player;

        [SerializeField] private float debugMoveSpeedWithReduction;
        
        public float GetMoveSpeed()
        {
            var currentMoveSpeed = _moveSpeed - _player.GetMovementReduction();
            debugMoveSpeedWithReduction = currentMoveSpeed;
            return currentMoveSpeed;
        }
    }
}