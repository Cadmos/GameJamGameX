using UnityEngine;

namespace FGJ24.Player
{
    public class PlayerIdleState : PlayerBaseState
    {
        private PlayerCharacter _character;
        private PlayerController _controller;
        private static readonly int StateEnum = Animator.StringToHash("StateEnum");

        public PlayerIdleState(PlayerCharacter character, PlayerController controller)
        {
            _character = character;
            _controller = controller;
        }

        public override void EnterState(PlayerStateManager player)
        {
        }
        public override void UpdateState(PlayerStateManager player)
        {
            if (!_controller.IsGrounded())
            {
                _controller.SetWasGroundedLastFrame(false);
                return;
            }
            
            if (PlayerControls.Instance.moveData.movePerformed)
            {
                player.SwitchState(player.GetPlayerMoveState());
            }
            
            if(PlayerControls.Instance.jumpData.jumpPerformed && _controller.GetNextJumpTime() <= Time.time)
                player.SwitchState(player.GetPlayerJumpState());
            
            if (PlayerControls.Instance.dashData.dashPerformed && _controller.GetNextDashTime() <= Time.time)
                player.SwitchState(player.GetPlayerDashState());
        }
        public override void FixedUpdateState(PlayerStateManager player)
        {
            _controller.StopHorizontalMovement();
        }
        public override void LateUpdateState(PlayerStateManager player)
        {
            _character.GetPlayerAnimator().Rotate(_controller.GetLastMovementDirection(), _character.GetPlayerCharacterAttributes().GetCharacterTurn().GetTurnSpeed());
            _character.GetPlayerAnimator().GetAnimator().SetInteger(StateEnum, (int)PlayerStateEnum.Idle);
        }
    }
}