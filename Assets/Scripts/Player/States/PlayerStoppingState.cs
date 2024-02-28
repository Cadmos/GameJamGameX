using UnityEngine;

namespace FGJ24.Player
{
    public class PlayerStoppingState : PlayerBaseState
    {
        public PlayerStoppingState(CharacterObject character, PlayerController controller) : base(character, controller)
        {
        }

        public override void EnterState(PlayerStateManager player)
        {
            Stopping();
            player.SetCurrentStateEnum(PlayerStateEnum.Stopping);
            _character.GetCharacterAnimator().GetAnimator().SetInteger(StateEnum, (int)PlayerStateEnum.Stopping);
        }

        public override void UpdateState(PlayerStateManager player)
        {

        }

        public override void FixedUpdateState(PlayerStateManager player)
        {
            _controller.UpdateGravity();
            _controller.UpdatePhysicsState();
            
            if (_controller.GetIsGrounded())
            {
                if (PlayerControls.Instance.moveData.movePerformed)
                {
                    player.SwitchState(player.GetPlayerMoveState());
                    return;
                }
                if (_controller.GetVelocity().magnitude < 0.1f)
                {
                    player.SwitchState(player.GetPlayerIdleState());
                    return;
                }
                Stopping();
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
            _character.RotateCharacter(_controller.GetVelocity(), _character.GetCharacterAttributes().GetCharacterStoppingStats().GetStoppingTurnSpeed(),_controller.GetUpAxis());
        }

        public override void ExitState(PlayerStateManager player)
        {
            player.SetPreviousStateEnum(PlayerStateEnum.Stopping);
        }

        private void Stopping()
        {
            _controller.Move(_controller.GetVelocity(), _character.GetCharacterAttributes().GetCharacterStoppingStats().GetStoppingAcceleration(), Vector2.zero, _character.GetCharacterAttributes().GetCharacterStoppingStats().GetStoppingSpeed());
        }
    }
}