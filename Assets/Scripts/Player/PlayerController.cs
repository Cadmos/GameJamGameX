using System;
using UnityEngine;

namespace FGJ24.Player
{
    [Serializable]
    public class PlayerController
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Collider _collider;
        
        public void Move(Vector3 direction, float movementSpeed)
        {
            Debug.Log("PlayerController.Move");
            _rigidbody.MovePosition(_rigidbody.position + direction * movementSpeed * Time.deltaTime);
        }
    }
}