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
        private bool _weJumped;

        private float _jumpStartTime;
        
        
        private bool _isJumping;
        private static readonly int StateEnum = Animator.StringToHash("StateEnum");

        public PlayerJumpState(PlayerCharacter character, PlayerController controller)
        {
            _character = character;
            _controller = controller;
        }
        
        public override void EnterState(PlayerStateManager player)
        {
            _jumpStartTime = Time.time;
            _isJumping = false;
            _jumpCurve = _character.GetPlayerCharacterAttributes().GetPlayerJump().GetJumpCurve();
            _curveDuration = _character.GetPlayerCharacterAttributes().GetPlayerJump().GetJumpCurveDuration();
            _curveTime = 0;
            _intentToJump = true;
            _movementSpeed = _character.GetPlayerCharacterAttributes().GetPlayerMoveSpeed().GetMoveSpeed();
            _movementDampener = _character.GetPlayerCharacterAttributes().GetPlayerJump().GetMovementDampening();
            _momentum = _controller.GetRigidbodyVelocity();
            _timeWhenLostControl = Time.time + _character.GetPlayerCharacterAttributes().GetPlayerJump().GetControllableAirDuration();
            _jumpForce = _character.GetPlayerCharacterAttributes().GetPlayerJump().GetJumpForce();
            _character.GetPlayerAnimator().GetAnimator().SetInteger(StateEnum, (int)PlayerStateEnum.Jump);
        }
        
        public override void UpdateState(PlayerStateManager player)
        {
            Vector3 moveDirection = new Vector3(PlayerControls.Instance.moveData.moveValue.x, 0, PlayerControls.Instance.moveData.moveValue.y);
            _moveInput = _controller.GetCameraTransform().forward * moveDirection.z + _controller.GetCameraTransform().right * moveDirection.x;
            _moveInput.y = 0;
            _movementSpeed = _character.GetPlayerCharacterAttributes().GetPlayerMoveSpeed().GetMoveSpeed();
            _controller.SetLastMovementDirection(_moveInput);
            
            _controller.SetDesiredVelocity(_moveInput * _movementSpeed);
                        
            if (_controller.GetRigidbodyVelocity().y > 0 || _controller.GetIsGrounded() == false || _intentToJump)
            {
                return;
            }
            
            if(PlayerControls.Instance.moveData.movePerformed)
            {
                player.SwitchState(player.GetPlayerMoveState());
            }
            player.SwitchState(player.GetPlayerIdleState());


 


            
            
            /*
            if (_intentToJump)
            {
                if (_controller.GetIsGrounded())
                {
                    Vector3 moveDirection = new Vector3(PlayerControls.Instance.moveData.moveValue.x, 0, PlayerControls.Instance.moveData.moveValue.y);
                    _moveInput = _controller.GetCameraTransform().forward*moveDirection.z + _controller.GetCameraTransform().right*moveDirection.x;
                    _moveInput.y = 0;
                    _movementSpeed = _character.GetPlayerCharacterAttributes().GetPlayerMoveSpeed().GetMoveSpeed();
            
                    _curveTime += Time.deltaTime/_curveDuration;
                    float curveValue = _jumpCurve.Evaluate(_curveTime);
                    _momentum = Vector3.Lerp(_moveInput, Vector3.zero, curveValue);

                    _controller.SetLastMovementDirection(_moveInput);
                    return;
                }
                
                if (PlayerControls.Instance.moveData.movePerformed)
                {
                    Vector3 moveDirection = new Vector3(PlayerControls.Instance.moveData.moveValue.x, 0, PlayerControls.Instance.moveData.moveValue.y);
                    _moveInput = _controller.GetCameraTransform().forward*moveDirection.z + _controller.GetCameraTransform().right*moveDirection.x;
                    _moveInput.y = 0;
                    _movementSpeed = _character.GetPlayerCharacterAttributes().GetPlayerMoveSpeed().GetMoveSpeed();
                    _controller.SetLastMovementDirection(_moveInput);
                    
                    player.SwitchState(player.GetPlayerMoveState());
                }
                player.SwitchState(player.GetPlayerIdleState());
                
            }

            if (_controller.GetRigidbodyVelocity().y > 0)
            {
                _isJumping = true;
                Vector3 moveDirection = new Vector3(PlayerControls.Instance.moveData.moveValue.x, 0, PlayerControls.Instance.moveData.moveValue.y);
                _moveInput = _controller.GetCameraTransform().forward*moveDirection.z + _controller.GetCameraTransform().right*moveDirection.x;
                _moveInput.y = 0;
                _movementSpeed = _character.GetPlayerCharacterAttributes().GetPlayerMoveSpeed().GetMoveSpeed();
            
                _curveTime += Time.deltaTime/_curveDuration;
                float curveValue = _jumpCurve.Evaluate(_curveTime);
                _momentum = Vector3.Lerp(_moveInput, Vector3.zero, curveValue);

                _controller.SetLastMovementDirection(_moveInput);
                return;
            }

            if (_controller.GetIsGrounded())
            {
                if (PlayerControls.Instance.moveData.movePerformed)
                {
                    Vector3 moveDirection = new Vector3(PlayerControls.Instance.moveData.moveValue.x, 0, PlayerControls.Instance.moveData.moveValue.y);
                    _moveInput = _controller.GetCameraTransform().forward*moveDirection.z + _controller.GetCameraTransform().right*moveDirection.x;
                    _moveInput.y = 0;
                    _movementSpeed = _character.GetPlayerCharacterAttributes().GetPlayerMoveSpeed().GetMoveSpeed();
            
                    _curveTime += Time.deltaTime/_curveDuration;
                    float curveValue = _jumpCurve.Evaluate(_curveTime);
                    _momentum = Vector3.Lerp(_moveInput, Vector3.zero, curveValue);

                    _controller.SetLastMovementDirection(_moveInput);
                    player.SwitchState(player.GetPlayerMoveState());
                }
                player.SwitchState(player.GetPlayerIdleState());
            }
            else
            {
                _isJumping = true;
                Vector3 moveDirection = new Vector3(PlayerControls.Instance.moveData.moveValue.x, 0, PlayerControls.Instance.moveData.moveValue.y);
                _moveInput = _controller.GetCameraTransform().forward*moveDirection.z + _controller.GetCameraTransform().right*moveDirection.x;
                _moveInput.y = 0;
                _movementSpeed = _character.GetPlayerCharacterAttributes().GetPlayerMoveSpeed().GetMoveSpeed();
            
                _curveTime += Time.deltaTime/_curveDuration;
                float curveValue = _jumpCurve.Evaluate(_curveTime);
                _momentum = Vector3.Lerp(_moveInput, Vector3.zero, curveValue);

                _controller.SetLastMovementDirection(_moveInput);
                return;
            }
            */
            
        }
        
        public override void FixedUpdateState(PlayerStateManager player)
        {
            _controller.AdjustVelocityAlongSurface();
            if (_intentToJump)
            {
                _intentToJump = false;
                Vector3 jumpDirection;
                if (_controller.GetIsGrounded())
                {
                    jumpDirection = _controller.GetContactNormal();
                }
                else if (_controller.GetIsSteep())
                {
                    jumpDirection = _controller.GetSteepNormal();
                    _controller.SetJumpPhase(0);
                }
                else if (_controller.GetMaxAirJumps() > 0 && _controller.GetJumpPhase() <= _controller.GetMaxAirJumps())
                {
                    if (_controller.GetJumpPhase() == 0)
                    {
                        _controller.SetJumpPhase(1);
                    }

                    jumpDirection = _controller.GetContactNormal();
                }
                else
                {
                    player.SwitchState(player.GetPlayerIdleState());
                    return;
                }

                _controller.SetNextJumpTime(Time.time + _character.GetPlayerCharacterAttributes().GetPlayerJump().GetJumpCooldownAfterJumping());
                _controller.Jump2(jumpDirection);
                
            }


            /*
            if (_intentToJump && _controller.GetIsGrounded() && _controller.GetNextJumpTime() <= Time.time)
            {
                Debug.Log("PlayerJumpState.FixedUpdateState");
                _controller.Jump(_jumpForce);
                _controller.Move(_momentum, _movementSpeed);
                _controller.SetNextJumpTime( Time.time + _character.GetPlayerCharacterAttributes().GetPlayerJump().GetJumpCooldownAfterJumping());
                _intentToJump = false;
                return;
            }

            if(_isJumping)
            {
                _controller.Move(_momentum, _movementSpeed);
            }
            */
        }
        
        public override void LateUpdateState(PlayerStateManager player)
        {
          
        }
    }
}