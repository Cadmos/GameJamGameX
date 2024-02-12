using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace FGJ24.Player
{
    [Serializable]
    public class PlayerCharacterAttributes
    {
        [SerializeField] private CharacterHealth _characterHealth;
        
        [SerializeField] private CharacterAttackDamage _characterAttackDamage;
        [SerializeField] private CharacterInteractionRange _characterInteractionRange;
        
        [SerializeField] private CharacterIdleStats _characterIdle;
        [SerializeField] private CharacterMoveStats _characterMove;
        [SerializeField] private CharacterDashStats _characterDash;
        [SerializeField] private CharacterJumpStats _characterJump;
        
        [SerializeField] private CharacterFallingStats _characterFalling;
        [SerializeField] private CharacterLandingStats _characterLanding;
        [SerializeField] private CharacterStoppingStats _characterStopping;
        [SerializeField] private CharacterSlidingStats _characterSliding;
        
        [SerializeField] private CharacterGatherStats _characterGather;
        [SerializeField] private CharacterMineStats _characterMine;
        [SerializeField] private CharacterCraftStats _characterCraft;
        
        [SerializeField] private CharacterDieStats _characterDie;
        [SerializeField] private CharacterWinStats _characterWin;
        
        public CharacterHealth GetCharacterHealth()
        {
            return _characterHealth;
        }
        public CharacterAttackDamage GetCharacterAttackDamage()
        {
            return _characterAttackDamage;
        }
        public CharacterInteractionRange GetCharacterInteractionRange()
        {
            return _characterInteractionRange;
        }
        public CharacterIdleStats GetCharacterIdleStats()
        {
            return _characterIdle;
        }
        public CharacterMoveStats GetCharacterMoveStats()
        {
            return _characterMove;
        }
        public CharacterDashStats GetCharacterDashStats()
        {
            return _characterDash;
        }
        public CharacterJumpStats GetCharacterJumpStats()
        {
            return _characterJump;
        }
        public CharacterFallingStats GetCharacterFallingStats()
        {
            return _characterFalling;
        }
        public CharacterLandingStats GetCharacterLandingStats()
        {
            return _characterLanding;
        }
        public CharacterStoppingStats GetCharacterStoppingStats()
        {
            return _characterStopping;
        }
        public CharacterSlidingStats GetCharacterSlidingStats()
        {
            return _characterSliding;
        }
        public CharacterGatherStats GetCharacterGatherStats()
        {
            return _characterGather;
        }
        public CharacterMineStats GetCharacterMineStats()
        {
            return _characterMine;
        }
        public CharacterCraftStats GetCharacterCraftStats()
        {
            return _characterCraft;
        }
        public CharacterDieStats GetCharacterDieStats()
        {
            return _characterDie;
        }
        public CharacterWinStats GetCharacterWinStats()
        {
            return _characterWin;
        }
    }
}