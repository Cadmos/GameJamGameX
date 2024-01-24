using System;
using UnityEngine;

namespace FGJ24.Player
{
    [Serializable]
    public class PlayerController
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Collider _collider;
    }
}