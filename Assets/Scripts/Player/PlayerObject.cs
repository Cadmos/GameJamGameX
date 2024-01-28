using System;
using UnityEngine;

namespace FGJ24.Player
{
    [Serializable]
    public class PlayerObject : MonoBehaviour
    {
        [SerializeField] private PlayerStateManager _playerStateManager;

        private void OnCollisionEnter(Collision collision)
        {
            _playerStateManager.GetController().EvaluateCollisions(collision);
        }

        private void OnCollisionStay(Collision collision)
        {
            _playerStateManager.GetController().EvaluateCollisions(collision);
        }

        private void Awake()
        {
            _playerStateManager.Initialize();
        }

        private void Update()
        {
            _playerStateManager.Update();
        }
        
        private void FixedUpdate()
        {
            _playerStateManager.FixedUpdate();
        }
        
        private void LateUpdate()
        {
            _playerStateManager.LateUpdate();
        }
    }
}