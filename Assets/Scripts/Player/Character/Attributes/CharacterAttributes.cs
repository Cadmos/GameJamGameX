using UnityEngine;

namespace FGJ24.Player
{
    public abstract class CharacterAttributes
    {
        protected GameCharacter _character;

    }

    public class GameCharacter
    {
        protected CharacterAttributes _characterAttributes;
        private CharacterAnimator _characterAnimator;

        public GameCharacter(CharacterAttributes characterAttributes, CharacterAnimator characterAnimator)
        {
            _characterAttributes = characterAttributes;
            _characterAnimator = characterAnimator;
        }
        
        public CharacterAttributes GetCharacterAttributes()
        {
            return _characterAttributes;
        }

        public CharacterAnimator GetCharacterAnimator()
        {
            return _characterAnimator;
        }

        public void RotateCharacter(Vector3 direction, float turnSpeed, Vector3 upAxis)
        {
            _characterAnimator.Rotate(direction, turnSpeed, upAxis);
        }

        public void ContactAlignedRotate(Vector3 contactNormal, Vector3 velocity, float turnSpeed)
        {
            _characterAnimator.ContactAlignedRotate(contactNormal, velocity, turnSpeed);
        }
    }
    
}