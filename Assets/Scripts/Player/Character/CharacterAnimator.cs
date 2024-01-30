using System;
using UnityEngine;

namespace FGJ24.Player
{
    [Serializable]
    public class CharacterAnimator
    {
        [SerializeField] private Animator _animator;
        
        [SerializeField] private AnimationClip _dashAnimation;
        [SerializeField] private AnimationClip _jumpAnimation;
        
        
        public AnimationClip GetDashAnimation()
        {
            return _dashAnimation;
        }
        
        public AnimationClip GetJumpAnimation()
        {
            return _jumpAnimation;
        }
        
        public Animator GetAnimator()
        {
            return _animator;
        }
        
        public void Rotate(Vector3 direction, float turnSpeed)
        {
            Vector3 horizontalVelocity = new Vector3(direction.x, 0, direction.z);
            var targetRotation = Quaternion.LookRotation(horizontalVelocity);
            _animator.transform.rotation = Quaternion.Lerp(_animator.transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }

    }
}