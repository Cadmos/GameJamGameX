using System;
using UnityEngine;

namespace FGJ24.Player
{
    [Serializable]
    public class CharacterAnimator
    {
        [SerializeField] private Animator _animator;
        
        public Animator GetAnimator()
        {
            return _animator;
        }

        public CharacterAnimator(Animator animator)
        {
            _animator = animator;
        }
        
        public void Rotate(Vector3 direction, float turnSpeed)
        {
            var targetRotation = Quaternion.LookRotation(direction);
            _animator.transform.rotation = Quaternion.Lerp(_animator.transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }

    }
}