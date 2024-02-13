using System;
using System.Collections;
using System.Collections.Generic;
using FGJ24.Player;
using UnityEngine;

namespace FGJ24
{
    public class DebugTransform : MonoBehaviour
    {
        public static DebugTransform Instance;
        
        [SerializeField] private Transform _forwardTransform;
        [SerializeField] private Transform _rightTransform;
        [SerializeField] private Transform _upTransform;
        [SerializeField] private Transform _nextFrameCapsuleTransform;
        
        public void SetForwardTransform(Vector3 direction)
        {
            _forwardTransform.forward = direction;
        }
        
        public void SetRightTransform(Vector3 direction)
        {
            _rightTransform.right = direction;
        }

        
        public void SetUpTransform(Vector3 direction)
        {
            _upTransform.up = direction;
        }
        
        public void SetNextFrameCapsuleTransform(Vector3 position)
        {
            _nextFrameCapsuleTransform.position = position;
        }


        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
