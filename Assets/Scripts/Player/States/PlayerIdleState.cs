using UnityEngine;

namespace FGJ24.Player
{
    public class PlayerIdleState : PlayerBaseState
    {
        public PlayerIdleState(CharacterObject character, PlayerController controller) : base(character, controller)
        {
        }

        public override void EnterState(PlayerStateManager player)
        {
            Idle();
            player.SetCurrentStateEnum(PlayerStateEnum.Idle);
            _character.GetCharacterAnimator().GetAnimator().SetInteger(StateEnum, (int)PlayerStateEnum.Idle);
        }

        public override void UpdateState(PlayerStateManager player)
        {

        }

        public override void FixedUpdateState(PlayerStateManager player)
        {
            _controller.UpdateGravity();
            _controller.PrePhysicsUpdate();
            
            if (_controller.GetIsGrounded())
            {
                if (PlayerControls.Instance.jumpData.jumpPerformed)
                {
                    player.SwitchState(player.GetPlayerJumpState());
                    return;
                }
                if (PlayerControls.Instance.moveData.movePerformed)
                {
                    player.SwitchState(player.GetPlayerMoveState());
                    return;
                }
                Idle();
                return;
            }

            if (_controller.GetIsSteep())
            {
                player.SwitchState(player.GetPlayerSlidingState());
                return;
            }
 
            player.SwitchState(player.GetPlayerFallingState());
        }

        public override void LateUpdateState(PlayerStateManager player)
        {
            if(_controller.GetVelocity().x != 0f && _controller.GetVelocity().z != 0f)
                _character.RotateCharacter( _controller.GetVelocity(), _character.GetCharacterAttributes().GetCharacterIdleStats().GetIdleTurnSpeed(),_controller.GetUpAxis());
        }

        public override void ExitState(PlayerStateManager player)
        {
            player.SetPreviousStateEnum(PlayerStateEnum.Idle);
        }

        private void Idle()
        {
            _controller.Move(_controller.GetVelocity(), _character.GetCharacterAttributes().GetCharacterIdleStats().GetIdleAcceleration(), Vector2.zero, 0);
        }
    }
}