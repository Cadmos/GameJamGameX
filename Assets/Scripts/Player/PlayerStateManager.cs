using System;
using UnityEngine;

namespace FGJ24.Player
{
    [Serializable]
    public class PlayerStateManager
    {
        [SerializeField] private PlayerController _controller;
        [SerializeField] private PlayerCharacter _character;
        
        private PlayerBaseState _currentState;
        
        private PlayerIdleState _idleState;
        private PlayerMoveState _moveState;
        private PlayerDashState _dashState;
        private PlayerJumpState _jumpState;
        
        #region Constructors
        #region Get

        public PlayerController GetController()
        {
            return _controller;
        }
        public PlayerIdleState GetPlayerIdleState()
        {
            return _idleState;
        }
        public PlayerMoveState GetPlayerMoveState()
        {
            return _moveState;
        }
        
        public PlayerDashState GetPlayerDashState()
        {
            return _dashState;
        }
        
        public PlayerJumpState GetPlayerJumpState()
        {
            return _jumpState;
        }

        
        #endregion
        #region Set

        public bool SetPlayerIdleState(PlayerIdleState playerIdleState)
        {
            _idleState = playerIdleState;
            return true;
        }

        public bool SetPlayerMoveState(PlayerMoveState playerMoveState)
        {
            _moveState = playerMoveState;
            return true;
        }
        public bool SetPlayerDashState(PlayerDashState playerDashState)
        {
            _dashState = playerDashState;
            return true;
        }
        public bool SetPlayerJumpState(PlayerJumpState playerJumpState)
        {
            _jumpState = playerJumpState;
            return true;
        }
        
        #endregion
        #endregion

        public void Initialize()
        {
            _controller.CalculateMinGroundDotProduct();
            _idleState = new PlayerIdleState(_character, _controller);
            _moveState = new PlayerMoveState(_character, _controller);
            _dashState = new PlayerDashState(_character, _controller);
            _jumpState = new PlayerJumpState(_character, _controller);
            
            //TODO better start with state resolving
            SwitchState(_idleState);
        }

        public void Update()
        {
            
            _currentState.UpdateState(this);
        }

        public void FixedUpdate()
        {
            
            _controller.UpdateState();
            
            _currentState.FixedUpdateState(this);
            _controller.UpdateRigidBodyVelocity();
            Debug.Log("currentState" + _currentState + " IsGrounded" + _controller.GetIsGrounded());
            _controller.ClearState();
        }

        public void LateUpdate()
        {
            _currentState.LateUpdateState(this);
            
            if(_controller.GetRigidbodyVelocity().magnitude > 0.1f)
            {
                Vector3 horizontalVelocity;
                horizontalVelocity = new Vector3(_controller.GetRigidbodyVelocity().x, 0, _controller.GetRigidbodyVelocity().z);
                _character.GetPlayerAnimator().Rotate(horizontalVelocity.normalized, _character.GetPlayerCharacterAttributes().GetCharacterTurn().GetTurnSpeed());
            }

          }
        
        public void SwitchState(PlayerBaseState state)
        {
            _currentState = state;
            state.EnterState(this);
        }
        
    }
}