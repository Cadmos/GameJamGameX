using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using Unity.Mathematics;
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
        
        public void Rotate(float3 direction, float turnSpeed)
        {
            float3 horizontalVelocity = new float3(direction.x, 0, direction.z);
            var targetRotation = Quaternion.LookRotation(horizontalVelocity);
            _animator.transform.rotation = Quaternion.Lerp(_animator.transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }
        /*
        public void ContactAlignedRotate(float3 contactNormal, float3 direction, float turnSpeed)
        {
            var transform = _animator.transform;
            
            float3 horizontalVelocity = new float3(direction.x, 0, direction.z);
            
            float3 right = math.normalize(math.cross(contactNormal, horizontalVelocity));
            float3 forward = math.normalize(math.cross(right, contactNormal));
            
            quaternion targetRotation = quaternion.LookRotationSafe(forward, contactNormal);
            
            transform.rotation = math.slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }
        */
        
        
        public void ContactAlignedRotate(float3 contactNormal, float3 direction, float turnSpeed)
        {
            // Create a NativeArray to hold the rotation
            var rotations = new NativeArray<quaternion>(1, Allocator.TempJob);
            rotations[0] = _animator.transform.rotation;

            var job = new ContactAlignedRotateJob
            {
                contactNormal = contactNormal,
                direction = direction,
                rotations = rotations,
                deltaTime = Time.deltaTime,
                turnSpeed = turnSpeed
            };

            // Schedule the job and immediately complete it
            job.Schedule().Complete();

            // Apply the calculated rotation to the transform
            _animator.transform.rotation = rotations[0];

            // Dispose of the NativeArray when you're done with it
            rotations.Dispose();
        }
        
    }
    
    [BurstCompile]
    public struct ContactAlignedRotateJob : IJob
    {
        public float3 contactNormal;
        public float3 direction;
        public NativeArray<quaternion> rotations;
        public float deltaTime;
        public float turnSpeed;

        public void Execute()
        {
            float3 horizontalVelocity = new float3(direction.x, 0, direction.z);
            float3 right = math.normalize(math.cross(contactNormal, horizontalVelocity));
            float3 forward = math.normalize(math.cross(right, contactNormal));
            quaternion targetRotation = quaternion.LookRotationSafe(forward, contactNormal);
            rotations[0] = math.slerp(rotations[0], targetRotation, turnSpeed * deltaTime);
        }
    }
}