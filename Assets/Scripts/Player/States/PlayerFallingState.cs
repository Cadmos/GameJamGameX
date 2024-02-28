using UnityEngine;

namespace FGJ24.Player
{
    public class PlayerFallingState : PlayerBaseState
    {
        public PlayerFallingState(CharacterObject character, PlayerController controller) : base(character, controller)
        {
        }

        public override void EnterState(PlayerStateManager player)
        {
            player.SetCurrentStateEnum(PlayerStateEnum.Falling);
            _character.GetCharacterAnimator().GetAnimator().SetInteger(StateEnum, (int)PlayerStateEnum.Falling);
            Falling();
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
                if (PlayerControls.Instance.moveData.movePerformed == false)
                {
                    player.SwitchState(player.GetPlayerStoppingState());
                    return;
                }
                player.SwitchState(player.GetPlayerLandingState());
                return;
            }

            if (_controller.GetIsSteep())
            {
                player.SwitchState(player.GetPlayerSlidingState());
                return;
            }
            Falling();
        }

        public override void LateUpdateState(PlayerStateManager player)
        {
            //_character.RootRotateCharacter(_controller.GetTargetRotation());
            _character.RotateCharacter( _controller.GetVelocity(), _character.GetCharacterAttributes().GetCharacterFallingStats().GetFallingTurnSpeed(), _controller.GetUpAxis());
        }

        public override void ExitState(PlayerStateManager player)
        {
            player.SetPreviousStateEnum(PlayerStateEnum.Falling);
        }

        private void Falling()
        {
            _controller.Falling(_controller.GetVelocity(), _character.GetCharacterAttributes().GetCharacterFallingStats().GetFallingMoveAcceleration(),PlayerControls.Instance.moveData.moveValue, _character.GetCharacterAttributes().GetCharacterFallingStats().GetFallingSpeed());
        }
    }
}