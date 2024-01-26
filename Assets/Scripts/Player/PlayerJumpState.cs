using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace FGJ24.Player
{
    public class PlayerJumpState : PlayerBaseState
    {
        private PlayerCharacter _character;
        private PlayerController _controller;
        
        private Vector3 _moveInput;
        private float _movementSpeed;

        private float _movementDampener;

        private float _momentumDampener;
        private Vector3 _momentum;
        private float _jumpForce;
        
        private float _timeWhenLostControl;
        
        private AnimationCurve _jumpCurve;
        private float _curveTime;
        private float _curveDuration;
        private bool _intentToJump;
        public PlayerJumpState(PlayerCharacter character, PlayerController controller)
        {
            _character = character;
            _controller = controller;
        }
        
        public override void EnterState(PlayerStateManager player)
        {
            _jumpCurve = _character.GetPlayerCharacterAttributes().GetPlayerJump().GetJumpCurve();
            _curveDuration = _character.GetPlayerCharacterAttributes().GetPlayerJump().GetJumpCurveDuration();
            _curveTime = 0;
            _intentToJump = true;
            _movementSpeed = _character.GetPlayerCharacterAttributes().GetPlayerMoveSpeed().GetMoveSpeed();
            _movementDampener = _character.GetPlayerCharacterAttributes().GetPlayerJump().GetMovementDampening();
            _momentum = _controller.GetRigidbodyVelocity();
            _timeWhenLostControl = Time.time + _character.GetPlayerCharacterAttributes().GetPlayerJump().GetControllableAirDuration();
            _jumpForce = _character.GetPlayerCharacterAttributes().GetPlayerJump().GetJumpForce();
        }
        
        public override void UpdateState(PlayerStateManager player)
        {
            if(_controller.IsGrounded() && _controller.GetWasGroundedLastFrame())
                player.SwitchState(player.GetPlayerIdleState());
            
            if (!_controller.IsGrounded())
            {
                _intentToJump = false;
                _controller.SetWasGroundedLastFrame(false);
            }

            if (_timeWhenLostControl <= Time.time)
            {
                _movementDampener = 0;
            }
            
            
            
            Vector3 moveDirection = new Vector3(PlayerControls.Instance.moveData.moveValue.x, 0, PlayerControls.Instance.moveData.moveValue.y);
            _moveInput = _controller.GetCameraTransform().forward*moveDirection.z + _controller.GetCameraTransform().right*moveDirection.x;
            _moveInput.y = 0;
            _movementSpeed = _character.GetPlayerCharacterAttributes().GetPlayerMoveSpeed().GetMoveSpeed();
            
            _curveTime += Time.deltaTime/_curveDuration;
            
            float curveValue = _jumpCurve.Evaluate(_curveTime);
            
            _momentum = Vector3.Lerp(_moveInput, Vector3.zero, curveValue);
            Debug.Log("_moveInput: " + _moveInput + " _momentum: " + _momentum);
            _controller.SetLastMovementDirection(_moveInput);

        }
        
        public override void FixedUpdateState(PlayerStateManager player)
        {
            if (_intentToJump && _controller.IsGrounded())
            {
                Debug.Log("PlayerJumpState.FixedUpdateState");
                _controller.Jump(_jumpForce);
                _intentToJump = false;
            }
            
            if(!_controller.IsGrounded())
            {
                _controller.Move(_momentum, _movementSpeed);
                
                
            }
            else if (!_controller.GetWasGroundedLastFrame())
            {
                _controller.SetWasGroundedLastFrame(true);
                _controller.Move(_moveInput, _movementSpeed);
                if(PlayerControls.Instance.moveData.movePerformed)
                    player.SwitchState(player.GetPlayerMoveState());
                else
                    player.SwitchState(player.GetPlayerIdleState());
            }
        }
        
        public override void LateUpdateState(PlayerStateManager player)
        {
            
        }
    }
}