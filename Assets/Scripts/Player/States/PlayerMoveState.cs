using UnityEngine;

namespace FGJ24.Player
{
    public class PlayerMoveState : PlayerBaseState
    {
        private PlayerCharacter _character;
        private PlayerController _controller;

        
        private Vector3 _moveInput;
        private float _movementSpeed;
        private static readonly int StateEnum = Animator.StringToHash("StateEnum");

        public PlayerMoveState(PlayerCharacter character, PlayerController controller)
        {
            _character = character;
            _controller = controller;
        }
        public override void EnterState(PlayerStateManager player)
        {
        }
        public override void UpdateState(PlayerStateManager player)
        {
            if (PlayerControls.Instance.moveData.movePerformed == false)
            player.SwitchState(player.GetPlayerIdleState());
            
            
            
            Vector3 moveDirection = new Vector3(PlayerControls.Instance.moveData.moveValue.x, 0, PlayerControls.Instance.moveData.moveValue.y);
            _moveInput = _controller.GetCameraTransform().forward*moveDirection.z + _controller.GetCameraTransform().right*moveDirection.x;
            _moveInput.y = 0;
            _movementSpeed = _character.GetPlayerCharacterAttributes().GetPlayerMoveSpeed().GetMoveSpeed();
            
            _controller.SetLastMovementDirection(_moveInput);
            
            if(PlayerControls.Instance.jumpData.jumpPerformed && _controller.GetNextJumpTime() <= Time.time)
                player.SwitchState(player.GetPlayerJumpState());
            
            if (PlayerControls.Instance.dashData.dashPerformed && _controller.GetNextDashTime() <= Time.time)
                player.SwitchState(player.GetPlayerDashState());
            

        }
        public override void FixedUpdateState(PlayerStateManager player)
        {
            _controller.Move(_moveInput, _movementSpeed);
        }
        public override void LateUpdateState(PlayerStateManager player)
        {
            _character.GetPlayerAnimator().Rotate(_controller.GetLastMovementDirection(), _character.GetPlayerCharacterAttributes().GetCharacterTurn().GetTurnSpeed());
            _character.GetPlayerAnimator().GetAnimator().SetInteger(StateEnum, (int)PlayerStateEnum.Move);
        }
    }
}