using System;
using UnityEngine;

namespace FGJ24.Player
{
    [Serializable]
    public class CameraLookSpeed
    {
        [SerializeField] private float _lookSpeed;
        
        public float GetLookSpeed()
        {
            return _lookSpeed;
        }
    }
}