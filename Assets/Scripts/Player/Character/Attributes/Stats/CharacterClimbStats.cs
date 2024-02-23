using System;
using UnityEngine;

namespace FGJ24.Player
{
    [Serializable]
    public class CharacterClimbStats
    {
        [SerializeField] private float _maxClimbSpeed;
        [SerializeField] private float _climbSpeed = 4f;
        [SerializeField] private float _acceleration = 100f;
        [SerializeField] private float _turnSpeed = 10f;


        public float GetMaxClimbSpeed()
        {
            return _maxClimbSpeed;
        }

        public float GetClimbSpeed()
        {
            return _climbSpeed;
        }

        public float GetAcceleration()
        {
            return _acceleration;
        }

        public float GetTurnSpeed()
        {
            return _turnSpeed;
        }

        public void SetMaxClimbSpeed(float maxClimbSpeed)
        {
            _maxClimbSpeed = maxClimbSpeed;
        }

        public void SetClimbSpeed(float climbSpeed)
        {
            _climbSpeed = climbSpeed;
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
