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
        private float _nextDashTime;
        private bool _intentToDash;
        public PlayerDashState(PlayerCharacter character, PlayerController controller)
        {
            _character = character;
            _controller = controller;
        }
        public override void EnterState(PlayerStateManager player)
        {
            Debug.Log("PlayerDashState.EnterState");
            _dashSpeed = _character.GetPlayerCharacterAttributes().GetPlayerDash().GetDashSpeed();
            _nextDashTime = _controller.GetNextDashTime();
            _intentToDash = true;
        }
        public override void UpdateState(PlayerStateManager player)
        {
            Vector3 moveDirection = new Vector3(PlayerControls.Instance.moveData.moveValue.x, 0, PlayerControls.Instance.moveData.moveValue.y);
            _moveInput = _controller.GetCameraTransform().forward*moveDirection.z + _controller.GetCameraTransform().right*moveDirection.x;
            _moveInput.y = 0;
            _movementSpeed = _character.GetPlayerCharacterAttributes().GetPlayerMoveSpeed().GetMoveSpeed();

            _controller.SetLastMovementDirection(_moveInput);
            
            if (_intentToDash && Time.time >= _nextDashTime)
                return;
            
            if(PlayerControls.Instance.moveData.movePerformed == false)
                player.SwitchState(player.GetPlayerIdleState());
            
            player.SwitchState(player.GetPlayerMoveState());
        }
        public override void FixedUpdateState(PlayerStateManager player)
        {
            if (_intentToDash && Time.time >= _nextDashTime)
            {
                _controller.Dash(_dashSpeed);
                _intentToDash = false;
                _controller.SetNextDashTime(Time.time + _character.GetPlayerCharacterAttributes().GetPlayerDash().GetDashCooldownDuration());
            }
            else
            {
                _controller.Move(_moveInput, _movementSpeed);
            }
             
        }
        public override void LateUpdateState(PlayerStateManager player)
        {
            
        }
    }
}