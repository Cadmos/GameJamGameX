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
            _idleState = new PlayerIdleState(_character, _controller);
            _moveState = new PlayerMoveState(_character, _controller);
            _dashState = new PlayerDashState(_character, _controller);
            _jumpState = new PlayerJumpState(_character, _controller);
            
            //TODO better start with state resolving
            SwitchState(_idleState);
        }

        public void Update()
        {
            Debug.Log($" Frame {Time.frameCount} Current State {_currentState}");
            _currentState.UpdateState(this);
        }

        public void FixedUpdate()
        {
            _currentState.FixedUpdateState(this);
        }

        public void LateUpdate()
        {
            _currentState.LateUpdateState(this);
        }
        
        public void SwitchState(PlayerBaseState state)
        {
            _currentState = state;
            state.EnterState(this);
        }
        
    }
}