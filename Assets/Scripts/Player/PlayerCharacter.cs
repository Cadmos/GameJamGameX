using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace FGJ24.Player
{
    [Serializable]
    public class CharacterObject
    {
        [SerializeField] private PlayerCharacterAttributes _characterAttributes;
        [SerializeField] private CharacterAnimator _characterAnimator;
        public PlayerCharacterAttributes GetCharacterAttributes()
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

        public void ContactAlignedRotate(Vector3 contactNormal , Vector3 velocity, float turnSpeed)
        {
            _characterAnimator.ContactAlignedRotate(contactNormal, velocity, turnSpeed);
        }
    }
}