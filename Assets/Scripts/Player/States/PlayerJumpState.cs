using UnityEngine;


namespace FGJ24.Player
{
    public class PlayerJumpState : PlayerBaseState
    {
        public PlayerJumpState(CharacterObject character, PlayerController controller) : base(character, controller)
        {
            
        }

        public override void EnterState(PlayerStateManager player)
        {
            player.SetCurrentStateEnum(PlayerStateEnum.Jump);
            _controller.SetIntentToJump(true);
            _character.GetCharacterAnimator().GetAnimator().SetInteger(StateEnum, (int)PlayerStateEnum.Jump);
        }

        public override void UpdateState(PlayerStateManager player)
        {
            _controller.UpdateDesiredVelocity(new Vector3(PlayerControls.Instance.moveData.moveValue.x,0, PlayerControls.Instance.moveData.moveValue.y), _character.GetCharacterAttributes().GetCharacterJumpStats().GetJumpSpeed());

            if (_controller.GetIntentToJump())
                return;

            if (_controller.GetVelocity().y > 0)
                return;

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
                    
                if (PlayerControls.Instance.dashData.dashPerformed && _controller.GetNextDashTime() <= Time.time)
                {
                    player.SwitchState(player.GetPlayerDashState());
                    return;
                }
                
                if (PlayerControls.Instance.moveData.movePerformed)
                {
                    player.SwitchState(player.GetPlayerMoveState());
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

            if(_controller.GetVelocity().y > -_character.GetCharacterAttributes().GetCharacterJumpStats().FallSpeedThreshold()) 
                return;
            
            player.SwitchState(player.GetPlayerFallingState());
        }

        public override void FixedUpdateState(PlayerStateManager player)
        {
            _controller.HorizontalMovement(_controller.GetVelocity(), _character.GetCharacterAttributes().GetCharacterJumpStats().GetJumpSpeed(), _controller.GetDesiredVelocity(), _controller.GetContactNormal());

            if (_controller.GetIntentToJump()) //TODO this is a bit of a hack, but it works for now
            {
                _controller.JumpToHeight(_controller.GetVelocity(),_controller.GetContactNormal(),_character.GetCharacterAttributes().GetCharacterJumpStats().GetJumpHeight());
                _controller.SetNextJumpTime(Time.time + _character.GetCharacterAttributes().GetCharacterJumpStats().GetJumpCooldownAfterJumping());
                _controller.SetIntentToJump(false);
            }
        }
        
        public override void LateUpdateState(PlayerStateManager player)
        {
            _character.RotateCharacter( _controller.GetVelocity(), _character.GetCharacterAttributes().GetCharacterJumpStats().GetJumpTurnSpeed());
        }

        public override void ExitState(PlayerStateManager player)
        {
            player.SetPreviousStateEnum(PlayerStateEnum.Jump);
        }
    }
}