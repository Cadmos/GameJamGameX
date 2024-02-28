using System;
using UnityEngine;

namespace FGJ24.Player
{
    [Serializable]
    public class CharacterDiveStats
    {
        [SerializeField] private float _maxDiveSpeed;
        [SerializeField] private float _diveSpeed = 4f;
        [SerializeField] private float _acceleration = 100f;
        [SerializeField] private float _turnSpeed = 10f;

        public float GetMaxDiveSpeed()
        {
            return _maxDiveSpeed;
        }
        public float GetDiveSpeed()
        {
            return _diveSpeed;
        }
        public float GetAcceleration()
        {
            return _acceleration;
        }
        public float GetTurnSpeed()
        {
            return _turnSpeed;
        }
        public void SetMaxDiveSpeed(float maxDiveSpeed)
        {
            _maxDiveSpeed = maxDiveSpeed;
        }
        public void SetDiveSpeed(float diveSpeed)
        {
            _diveSpeed = diveSpeed;
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