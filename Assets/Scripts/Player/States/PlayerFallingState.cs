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
            _character.GetCharacterAnimator().GetAnimator().SetInteger(StateEnum, (int)PlayerStateEnum.Falling);
        }

        public override void UpdateState(PlayerStateManager player)
        {
            if (_controller.GetIsGrounded())
            {
                if(_controller.HaveWeWon())
                {
                    player.SwitchState(player.GetPlayerWinState());
                    return;
                }
            
                if(_controller.HaveWeLost())
                {
                    player.SwitchState(player.GetPlayerDieState());
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
            
            _controller.SetDesiredVelocity(Vector3.zero);
        }

        public override void FixedUpdateState(PlayerStateManager player)
        {
            _controller.AdjustVelocity(_controller.GetVelocity(), _character.GetCharacterAttributes().GetCharacterFallingStats().GetFallingAcceleration(), _controller.GetDesiredVelocity());
            _controller.LimitVelocity(_character.GetCharacterAttributes().GetCharacterFallingStats().GetMaxFallingSpeed());
        }

        public override void LateUpdateState(PlayerStateManager player)
        {
            _character.RotateCharacter( _controller.GetVelocity(), _character.GetCharacterAttributes().GetCharacterFallingStats().GetFallingTurnSpeed());
        }

        public override void ExitState(PlayerStateManager player)
        {
            player.SetPreviousStateEnum(PlayerStateEnum.Falling);
        }
    }
}