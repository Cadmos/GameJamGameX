using System;
using UnityEngine;

namespace FGJ24.Player
{
    [Serializable]
    public class CharacterTurn
    {
        [SerializeField] private float _turnSpeed = 2f;
        public float GetTurnSpeed()
        {
            return _turnSpeed;
        }
    }
}