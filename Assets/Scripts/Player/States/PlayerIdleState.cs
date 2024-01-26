using UnityEngine;

namespace FGJ24.Player
{
    public class PlayerIdleState : PlayerBaseState
    {
        private PlayerCharacter _character;
        private PlayerController _controller;
        
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

            if (PlayerControls.Instance.jumpData.jumpPerformed)
            {
                player.SwitchState(player.GetPlayerJumpState());
            }
            
            if (PlayerControls.Instance.moveData.movePerformed)
            {
                player.SwitchState(player.GetPlayerMoveState());
            }
            
            if (PlayerControls.Instance.dashData.dashPerformed)
            {
                player.SwitchState(player.GetPlayerDashState());
            }
        }
        public override void FixedUpdateState(PlayerStateManager player)
        {
        }
        public override void LateUpdateState(PlayerStateManager player)
        {
            //Debug.Log("PlayerIdleState.LateUpdateState11111111");
            //CameraController.Instance.Look(PlayerControls.Instance.lookData.lookValue, _character.GetPlayerCharacterAttributes().GetPlayerLookSpeed().GetLookSpeed());
        }
    }
}