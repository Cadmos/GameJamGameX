using System;
using UnityEngine;

namespace FGJ24.Player
{
    [Serializable]
    public class PlayerCharacterAttributes
    {
        [SerializeField] private CharacterHealth _playerHealth;
        [SerializeField] private CharacterMoveSpeed _playerMoveSpeed;
        [SerializeField] private CharacterAttackDamage _playerAttackDamage;
        [SerializeField] private CharacterInteractionRange _playerInteractionRange;
        [SerializeField] private CharacterDash _playerDash;
        [SerializeField] private CharacterJump _playerJump;
        
        public CharacterHealth GetPlayerHealth()
        {
            return _playerHealth;
        }
        
        public CharacterMoveSpeed GetPlayerMoveSpeed()
        {
            return _playerMoveSpeed;
        }
        
        public CharacterAttackDamage GetPlayerAttackDamage()
        {
            return _playerAttackDamage;
        }
        
        public CharacterInteractionRange GetPlayerInteractionRange()
        {
            return _playerInteractionRange;
        }
        
        public CharacterJump GetPlayerJump()
        {
            return _playerJump;
        }

        public CharacterDash GetPlayerDash()
        {
            return _playerDash;
        }
    }
}