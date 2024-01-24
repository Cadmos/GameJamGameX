using System;
using UnityEngine;

namespace FGJ24.Player
{
    [Serializable]
    public class CharacterAnimator
    {
        [SerializeField] private Animator _animator;

        public CharacterAnimator(Animator animator)
        {
            _animator = animator;
        }
    }
}