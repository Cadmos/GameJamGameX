using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace FGJ24.Player
{
    [Serializable]
    public class CharacterMoveStats
    {
        [SerializeField] private float _maxMoveSpeed;
        [SerializeField] private float _moveSpeed = 20f;
        [SerializeField] private float _acceleration = 100f;
        [SerializeField] private float _turnSpeed = 10f;
        [SerializeField] private float _minMoveSpeed;
        [SerializeField] private Player _player;

        [SerializeField] private float _debugMoveSpeedWithReduction;
        
        public float GetMoveSpeed()
        {
            var currentMoveSpeed = _moveSpeed - _player.GetMovementReduction();
            _debugMoveSpeedWithReduction = currentMoveSpeed;
            return currentMoveSpeed;
        }
        public float GetAcceleration()
        {
            return _acceleration;
        }

        public float GetTurnSpeed()
        {
            return _turnSpeed;
        }
    }
}