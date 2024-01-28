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
            _character.GetPlayerAnimator().GetAnimator().SetInteger(StateEnum, (int)PlayerStateEnum.Idle);
        }
        public override void UpdateState(PlayerStateManager player)
        {
            if (_controller.GetIsGrounded())
            {
                Debug.Log("We are grounded");
                if (PlayerControls.Instance.moveData.movePerformed)
                {
                    player.SwitchState(player.GetPlayerMoveState());
                }

                if (PlayerControls.Instance.jumpData.jumpPerformed && _controller.GetNextJumpTime() <= Time.time)
                    player.SwitchState(player.GetPlayerJumpState());

                if (PlayerControls.Instance.dashData.dashPerformed && _controller.GetNextDashTime() <= Time.time)
                    player.SwitchState(player.GetPlayerDashState());
                
                _controller.SetDesiredVelocity(Vector3.zero);
            }
            else
            {
                //player.SwitchState(player.GetPlayerFallingState());
            }
        }
        public override void FixedUpdateState(PlayerStateManager player)
        {
            _controller.AdjustVelocityAlongSurface();
        }
        public override void LateUpdateState(PlayerStateManager player)
        {
        }
    }
}