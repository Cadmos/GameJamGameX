using System;
using UnityEngine;

namespace FGJ24.Player
{
    [Serializable]
    public class CharacterSwimStats
    {
        [SerializeField] private float _maxSwimSpeed;
        [SerializeField] private float _swimSpeed = 4f;
        [SerializeField] private float _acceleration = 100f;
        [SerializeField] private float _turnSpeed = 10f;

        public float GetMaxSwimSpeed()
        {
            return _maxSwimSpeed;
        }
        public float GetSwimSpeed()
        {
            return _swimSpeed;
        }
        public float GetAcceleration()
        {
            return _acceleration;
        }
        public float GetTurnSpeed()
        {
            return _turnSpeed;
        }
        public void SetMaxSwimSpeed(float maxSwimSpeed)
        {
            _maxSwimSpeed = maxSwimSpeed;
        }
        public void SetSwimSpeed(float swimSpeed)
        {
            _swimSpeed = swimSpeed;
        }
        public void SetAcceleration(float acceleration)
        {
            _acceleration = acceleration;
        }
        public void SetTurnSpeed(float turnSpeed)
        {
            _turnSpeed = turnSpeed;
        }
    }
}