using System;
using UnityEngine;

namespace FGJ24.Player
{
    [Serializable]
    public class PlayerCharacter
    {
        [SerializeField] private CharacterAnimator _playerAnimator;
        [SerializeField] private PlayerCharacterAttributes _playerCharacterAttributes;
    }
}