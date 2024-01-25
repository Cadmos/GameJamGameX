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
        
        #endregion
        #endregion

        public void InitializeStates()
        {
            //Debug.Log("Character State Manager Start");
            _idleState = new PlayerIdleState(_character, _controller);
            _moveState = new PlayerMoveState(_character, _controller);
            
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
            Debug.Log($" Frame {Time.frameCount} Current State {_currentState}");
            _currentState.LateUpdateState(this);
        }
        
        public void SwitchState(PlayerBaseState state)
        {
            _currentState = state;
            state.EnterState(this);
        }
        
    }
}