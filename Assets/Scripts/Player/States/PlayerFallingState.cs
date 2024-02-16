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
                
                if (PlayerControls.Instance.interactData.interactPerformed)
                {
                    player.SwitchState(player.GetPlayerGatherState());
                    return;
                }

                if (PlayerControls.Instance.jumpData.jumpPerformed && _controller.GetNextJumpTime() <= Time.time && _controller.GetJumpPhase() == 0)
                {     
                    player.SwitchState(player.GetPlayerJumpState());
                    return;
                }

                if (PlayerControls.Instance.dashData.dashPerformed && _controller.GetNextDashTime() <= Time.time)
                {
                    player.SwitchState(player.GetPlayerDashState());
                    return;
                }

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
            
            _controller.UpdateDesiredVelocity(new Vector3(PlayerControls.Instance.moveData.moveValue.x,_character.GetCharacterAttributes().GetCharacterFallingStats().GetFallingSpeed(), PlayerControls.Instance.moveData.moveValue.y),_character.GetCharacterAttributes().GetCharacterFallingStats().GetFallingMoveSpeed());
        }

        public override void FixedUpdateState(PlayerStateManager player)
        {
            _controller.Falling(_controller.GetVelocity(), _character.GetCharacterAttributes().GetCharacterFallingStats().GetFallingMoveAcceleration(),_character.GetCharacterAttributes().GetCharacterFallingStats().GetFallingAcceleration(), _controller.GetDesiredVelocity());
            //_controller.LimitVelocity(_character.GetCharacterAttributes().GetCharacterFallingStats().GetMaxFallingSpeed());
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