using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace FGJ24.Player
{
    [Serializable]
    public class CharacterObject
    {
        [SerializeField] private CharacterAttributes _characterAttributes;
        [SerializeField] private CharacterAnimator _characterAnimator;
        public CharacterAttributes GetCharacterAttributes()
        {
            return _characterAttributes;
        }
        public CharacterAnimator GetCharacterAnimator()
        {
            return _characterAnimator;
        }

        public void RotateCharacter(Vector3 direction, float turnSpeed)
        {
            _characterAnimator.Rotate(direction, turnSpeed);
        }
    }
}