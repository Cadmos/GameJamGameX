using System;
using UnityEngine;

namespace FGJ24.Player
{
    [Serializable]
    public class PlayerStateManager
    {
        [SerializeField] private PlayerController _controller;
        [SerializeField] private CharacterObject _character;

        private PlayerBaseState _currentState;
        private PlayerStateEnum _previousStateEnum;
        private PlayerStateEnum _currentStateEnum;

        private PlayerIdleState _idleState;
        private PlayerMoveState _moveState;
        private PlayerDashState _dashState;
        private PlayerJumpState _jumpState;
        
        private PlayerClimbState _climbState;
        private PlayerSwimState _swimState;
        
        private PlayerFallingState _fallingState;
        private PlayerLandingState _landingState;
        private PlayerStoppingState _stoppingState;
        private PlayerSlidingState _slidingState;
        
        private PlayerGatherState _gatherState;
        private PlayerMineState _mineState;
        private PlayerCraftState _craftState;
        
        private PlayerDieState _dieState;
        private PlayerWinState _winState;
        
        #region Get

        public PlayerController GetController()
        {
            return _controller;
        }

        public CharacterObject GetCharacter()
        {
            return _character;
        }

        public PlayerBaseState GetCurrentState()
        {
            return _currentState;
        }
        
        public PlayerStateEnum GetPreviousStateEnum()
        {
            return _previousStateEnum;
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
        
        public PlayerClimbState GetPlayerClimbState()
        {
            return _climbState;
        }
        
        public PlayerSwimState GetPlayerSwimState()
        {
            return _swimState;
        }
        
        public PlayerFallingState GetPlayerFallingState()
        {
            return _fallingState;
        }
        
        public PlayerSlidingState GetPlayerSlidingState()
        {
            return _slidingState;
        }
        
        public PlayerStoppingState GetPlayerStoppingState()
        {
            return _stoppingState;
        }
        
        public PlayerLandingState GetPlayerLandingState()
        {
            return _landingState;
        }
        
        public PlayerGatherState GetPlayerGatherState()
        {
            return _gatherState;
        }
        
        public PlayerMineState GetPlayerMineState()
        {
            return _mineState;
        }
        
        public PlayerCraftState GetPlayerCraftState()
        {
            return _craftState;
        }
        
        public PlayerDieState GetPlayerDieState()
        {
            return _dieState;
        }
        
        public PlayerWinState GetPlayerWinState()
        {
            return _winState;
        }
        #endregion

        public void Initialize()
        {
            _controller.CalculateMinStairsDotProduct();
            _controller.CalculateMinGroundDotProduct();
            _controller.CalculateMinSteepDotProduct();
            _controller.CalculateMinClimbDotProduct();
            
            _idleState = new PlayerIdleState(_character, _controller);
            _moveState = new PlayerMoveState(_character, _controller);
            _dashState = new PlayerDashState(_character, _controller);
            _jumpState = new PlayerJumpState(_character, _controller);
            _climbState = new PlayerClimbState(_character, _controller);
            _swimState = new PlayerSwimState(_character, _controller);
            
            _fallingState = new PlayerFallingState(_character, _controller);
            _slidingState = new PlayerSlidingState(_character, _controller);
            _stoppingState = new PlayerStoppingState(_character, _controller);
            _landingState = new PlayerLandingState(_character, _controller);
            
            _gatherState = new PlayerGatherState(_character, _controller);
            _mineState = new PlayerMineState(_character, _controller);
            _craftState = new PlayerCraftState(_character, _controller);
            
            _dieState = new PlayerDieState(_character, _controller);
            _winState = new PlayerWinState(_character, _controller);
            
            _currentState = _fallingState;
        }

        public void Update()
        {
            //Debug.Log("Update, Frame " + Time.frameCount + "State: " + _currentState);
            _currentState.UpdateState(this);
        }

        public void FixedUpdate()
        {
            Debug.Log("FixedUpdate, Frame " + Time.frameCount + "State: " + _currentState);
            _currentState.FixedUpdateState(this);
            DebugCanvasController.Instance.SetStateText(_currentStateEnum.ToString());
            _controller.UpdateRigidBody();
            _controller.ClearState();
        }

        public void LateUpdate()
        {
            _currentState.LateUpdateState(this);
        }

        public void SwitchState(PlayerBaseState state)
        {
            _currentState.ExitState(this);
            _currentState = state;
            state.EnterState(this);
        }

        public void SetPreviousStateEnum(PlayerStateEnum previousStateEnum)
        {
            _previousStateEnum = previousStateEnum;
        }
        
        public void SetCurrentStateEnum(PlayerStateEnum currentState)
        {
            _currentStateEnum = currentState;
        }
    }
}