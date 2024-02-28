using System;
using UnityEngine;

namespace FGJ24.Player
{
    [Serializable]
    public class CharacterSurfacingStats
    {
        [SerializeField] private float _maxSurfacingSpeed;
        [SerializeField] private float _surfacingSpeed = 4f;
        [SerializeField] private float _acceleration = 100f;
        [SerializeField] private float _turnSpeed = 10f;

        public float GetMaxDiveSpeed()
        {
            return _maxSurfacingSpeed;
        }
        public float GetDiveSpeed()
        {
            return _surfacingSpeed;
        }
        public float GetAcceleration()
        {
            return _acceleration;
        }
        public float GetTurnSpeed()
        {
            return _turnSpeed;
        }
        public void SetMaxDiveSpeed(float maxSurfacingSpeed)
        {
            _maxSurfacingSpeed = maxSurfacingSpeed;
        }
        public void SetDiveSpeed(float surfacingSpeed)
        {
            _surfacingSpeed = surfacingSpeed;
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