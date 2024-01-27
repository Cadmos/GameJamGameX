using System;
using UnityEngine;

namespace FGJ24.Player
{
    [Serializable]
    public class PlayerCharacter
    {
        [SerializeField] private CharacterAnimator _playerAnimator;
        [SerializeField] private PlayerCharacterAttributes _playerCharacterAttributes;
        
        public PlayerCharacterAttributes GetPlayerCharacterAttributes()
        {
            return _playerCharacterAttributes;
        }
        
        public CharacterAnimator GetPlayerAnimator()
        {
            return _playerAnimator;
        }
    }
}