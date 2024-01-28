using UnityEngine;

namespace FGJ24.Player
{
    public class PlayerDashState : PlayerBaseState
    {
        private PlayerCharacter _character;
        private PlayerController _controller;
        
        private Vector3 _moveInput;
        private float _movementSpeed;
        
        private float _dashSpeed;
        private float _dashAcceleration;
        private float _nextDashTime;
        private bool _intentToDash;

        private AnimationCurve _dashCurve;
        private float _dashCurveTime;
        private float _dashStartTime;
        private float _dashCurveDuration;
        
        
        private float _elapsedTimeDashing;
        private float _dashDuration;

        private bool _isDashing;
        private static readonly int StateEnum = Animator.StringToHash(("StateEnum"));
        private static readonly int DashAnimationSpeed = Animator.StringToHash("DashAnimationSpeed");

        public PlayerDashState(PlayerCharacter character, PlayerController controller)
        {
            _character = character;
            _controller = controller;
        }
        public override void EnterState(PlayerStateManager player)
        {
            _dashCurve = _character.GetPlayerCharacterAttributes().GetPlayerDash().GetDashCurve();
            _dashCurveDuration = _character.GetPlayerCharacterAttributes().GetPlayerDash().GetDashCurveDuration();
            _dashAcceleration = _character.GetPlayerCharacterAttributes().GetPlayerDash().GetDashAcceleration();
            _elapsedTimeDashing = 0;
            _dashDuration = _character.GetPlayerCharacterAttributes().GetPlayerDash().GetDashDuration();
            _isDashing = false;
            _movementSpeed = _character.GetPlayerCharacterAttributes().GetPlayerMoveSpeed().GetMoveSpeed();
            Debug.Log("PlayerDashState.EnterState");
            _dashSpeed = _character.GetPlayerCharacterAttributes().GetPlayerDash().GetDashSpeed();
            _nextDashTime = _controller.GetNextDashTime();
            _intentToDash = true;
            _dashStartTime = Time.time;
            _character.GetPlayerAnimator().GetAnimator().SetFloat(DashAnimationSpeed, 0.667f/_dashDuration);
            _controller.SetMaxSnapSpeed(_dashSpeed+1);
            
            _character.GetPlayerAnimator().GetAnimator().SetInteger(StateEnum, (int)PlayerStateEnum.Dash);
        }
        public override void UpdateState(PlayerStateManager player)
        {
            Vector3 moveDirection = new Vector3(PlayerControls.Instance.moveData.moveValue.x, 0, PlayerControls.Instance.moveData.moveValue.y);
            _moveInput = _controller.GetCameraTransform().forward * moveDirection.z + _controller.GetCameraTransform().right * moveDirection.x;
            _moveInput.y = 0;
            _movementSpeed = _character.GetPlayerCharacterAttributes().GetPlayerMoveSpeed().GetMoveSpeed();
            _controller.SetLastMovementDirection(_moveInput);


            _controller.SetDesiredVelocity(_moveInput * _dashSpeed);
            
            if(_dashStartTime + _dashDuration < Time.time)
                player.SwitchState(player.GetPlayerIdleState());

            /*
            if (_intentToDash)
            {
                if (_controller.GetNextDashTime() <= Time.time)
                {
                    if (PlayerControls.Instance.moveData.movePerformed)
                    {
                        Vector3 moveDirection = new Vector3(PlayerControls.Instance.moveData.moveValue.x, 0, PlayerControls.Instance.moveData.moveValue.y);
                        _moveInput = _controller.GetCameraTransform().forward*moveDirection.z + _controller.GetCameraTransform().right*moveDirection.x;
                        _moveInput.y = 0;
                        _movementSpeed = _character.GetPlayerCharacterAttributes().GetPlayerMoveSpeed().GetMoveSpeed();

                        _dashCurveTime += Time.deltaTime/_dashDuration;
                        float curveValue = _dashCurve.Evaluate(_dashCurveTime);
                        _moveInput = Vector3.Lerp(_moveInput, Vector3.zero, curveValue);

                        _controller.SetLastMovementDirection(_moveInput);
                        return;
                    }


                }

                if(_controller.GetNextDashTime() > Time.time)
                {
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

            }



            _elapsedTimeDashing += Time.deltaTime;
            //we just dashed

            //if our speed is faster than our movement speed then we are still dashing
            if (_controller.GetRigidbodyVelocity().magnitude > _movementSpeed && _dashDuration > _elapsedTimeDashing)
            {
                Debug.Log("Still dashing");
                Debug.Log(_controller.GetRigidbodyVelocity().magnitude + " > " + _movementSpeed);

                _dashCurveTime += Time.deltaTime/_dashDuration;
                float curveValue = _dashCurve.Evaluate(_dashCurveTime);
                _moveInput = Vector3.Lerp(_controller.GetRigidbodyVelocity(), Vector3.zero, curveValue);

                _isDashing = true;
                return;
            }

            //we are no longer dashing

            //now resolve other states

            if (PlayerControls.Instance.moveData.movePerformed)
            {
                Vector3 moveDirection = new Vector3(PlayerControls.Instance.moveData.moveValue.x, 0, PlayerControls.Instance.moveData.moveValue.y);
                _moveInput = _controller.GetCameraTransform().forward*moveDirection.z + _controller.GetCameraTransform().right*moveDirection.x;
                _moveInput.y = 0;
                _movementSpeed = _character.GetPlayerCharacterAttributes().GetPlayerMoveSpeed().GetMoveSpeed();
                _controller.SetLastMovementDirection(_moveInput);

                _dashCurveTime += Time.deltaTime/_dashDuration;
                float curveValue = _dashCurve.Evaluate(_dashCurveTime);
                _moveInput = Vector3.Lerp(_moveInput, Vector3.zero, curveValue);
                player.SwitchState(player.GetPlayerMoveState());
            }
            player.SwitchState(player.GetPlayerIdleState());
            */
        }
        public override void FixedUpdateState(PlayerStateManager player)
        {
            _controller.AdjustVelocityAlongSurface(_dashAcceleration);
          
            _controller.DashMoveVelocity(_moveInput, _dashSpeed);
            _controller.SetNextDashTime(Time.time + _character.GetPlayerCharacterAttributes().GetPlayerDash().GetDashCooldownDuration());
            _character.GetPlayerAnimator().GetAnimator().SetInteger(StateEnum, (int)PlayerStateEnum.Dash);

            

            /*

            Debug.Log("next dash time: " + _controller.GetNextDashTime() + " current time: " + Time.time);
            if (_intentToDash && _controller.GetNextDashTime() <= Time.time)
            {
                Debug.Log("PlayerDashState.FixedUpdateState");
                _controller.Dash(_dashSpeed);
                _controller.Move(_moveInput, _movementSpeed);
                _controller.SetNextDashTime(Time.time + _character.GetPlayerCharacterAttributes().GetPlayerDash().GetDashCooldownDuration());
                _intentToDash = false;


                return;
            }

            if(_isDashing)
                _controller.Move(_moveInput, _movementSpeed);
                */
        }
        public override void LateUpdateState(PlayerStateManager player)
        {

        }
    }
}