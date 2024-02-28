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
            InitialJump();
            player.SetCurrentStateEnum(PlayerStateEnum.Jump);
            _character.GetCharacterAnimator().GetAnimator().SetInteger(StateEnum, (int)PlayerStateEnum.Jump);
            _controller.SetIntentToJump(true);
        }

        public override void UpdateState(PlayerStateManager player)
        {
        }

        public override void FixedUpdateState(PlayerStateManager player)
        {
            _controller.UpdateGravity();
            _controller.PrePhysicsUpdate();

            if (_controller.GetIntentToJump())
            {
                Jump();
                _controller.SetIntentToJump(false);
                return;
            }
            
            Debug.Log("JumpState: " + _controller.IsGrounded);
            if (_controller.IsGrounded)
            {
                if (PlayerControls.Instance.moveData.movePerformed == false)
                {
                    player.SwitchState(player.GetPlayerStoppingState());
                    return;
                }
                
                if (PlayerControls.Instance.moveData.movePerformed)
                {
                    player.SwitchState(player.GetPlayerMoveState());
                    return;
                }
                return;
            }

            if (_controller.IsSteep)
            {
                player.SwitchState(player.GetPlayerSlidingState());
                return;
            }

            
            if (_controller.IsMovingUp())
            {
                Jump();
                return;
            }
            player.SwitchState(player.GetPlayerFallingState());
        }
        
        public override void LateUpdateState(PlayerStateManager player)
        {
            _character.RotateCharacter( _controller.GetVelocity(), _character.GetCharacterAttributes().GetCharacterJumpStats().GetJumpTurnSpeed(),_controller.GetUpAxis());
        }

        public override void ExitState(PlayerStateManager player)
        {
            player.SetPreviousStateEnum(PlayerStateEnum.Jump);
        }


        private void InitialJump()
        {
            _controller.Jump(_controller.GetVelocity(), _character.GetCharacterAttributes().GetCharacterJumpStats().GetJumpAcceleration(), PlayerControls.Instance.moveData.moveValue, _character.GetCharacterAttributes().GetCharacterJumpStats().GetJumpSpeed());
            _controller.JumpToHeight(_controller.GetVelocity(),_controller.GetContactNormal(),_character.GetCharacterAttributes().GetCharacterJumpStats().GetJumpHeight());
            _controller.SetNextJumpTime(Time.time + _character.GetCharacterAttributes().GetCharacterJumpStats().GetJumpCooldownAfterJumping());
        }

        private void Jump()
        {
            _controller.Jump(_controller.GetVelocity(), _character.GetCharacterAttributes().GetCharacterJumpStats().GetJumpAcceleration(), PlayerControls.Instance.moveData.moveValue, _character.GetCharacterAttributes().GetCharacterJumpStats().GetJumpSpeed());
        }
    }
}