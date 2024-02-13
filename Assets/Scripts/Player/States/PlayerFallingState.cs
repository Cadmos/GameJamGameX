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
        }

        public override void UpdateState(PlayerStateManager player)
        {
            _controller.UpdateDesiredVelocity(new Vector3(PlayerControls.Instance.moveData.moveValue.x,_character.GetCharacterAttributes().GetCharacterFallingStats().GetFallingSpeed(), PlayerControls.Instance.moveData.moveValue.y),_character.GetCharacterAttributes().GetCharacterFallingStats().GetFallingMoveSpeed());
            
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
        }

        public override void FixedUpdateState(PlayerStateManager player)
        {
            _controller.Falling(_controller.GetVelocity(), _character.GetCharacterAttributes().GetCharacterFallingStats().GetFallingAcceleration(), _controller.GetDesiredVelocity(), _controller.GetContactNormal());
            //_controller.LimitVelocity(_character.GetCharacterAttributes().GetCharacterFallingStats().GetMaxFallingSpeed());
        }

        public override void LateUpdateState(PlayerStateManager player)
        {
            Debug.Log("Falling LateUpdate");
            _character.RotateCharacter( _controller.GetVelocity(), _character.GetCharacterAttributes().GetCharacterFallingStats().GetFallingTurnSpeed());
        }

        public override void ExitState(PlayerStateManager player)
        {
            player.SetPreviousStateEnum(PlayerStateEnum.Falling);
        }
    }
}