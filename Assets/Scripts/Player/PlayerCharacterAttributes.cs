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
    }
}