using System;
using UnityEngine;

namespace FGJ24.Player
{
    [Serializable]
    public class PlayerController
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Collider _collider;
        [SerializeField] private Transform _cameraTransform;


        
        public Transform GetCameraTransform()
        {
            return _cameraTransform;
        }

        public void Initialize()
        {
        }
        
        public void Move(Vector3 direction, float movementSpeed)
        {
            

            
            _rigidbody.MovePosition(_rigidbody.position + direction * (movementSpeed * Time.deltaTime));
        }
    }
}