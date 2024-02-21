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

        public void RootRotateCharacter(Quaternion targetRotation)
        {
            _characterAnimator.RootRotation(targetRotation);
        }
        public void RotateCharacter(Vector3 direction, float turnSpeed, Vector3 upAxis)
        {
            _characterAnimator.Rotate(direction, turnSpeed, upAxis);
        }


        public void ContactAlignedRotate(Vector3 contactNormal , Vector3 velocity, float turnSpeed)
        {
            _characterAnimator.ContactAlignedRotate(contactNormal, velocity, turnSpeed);
        }
    }
}