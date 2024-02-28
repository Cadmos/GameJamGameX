using UnityEngine;

namespace FGJ24.Player
{
    public class PlayerMoveState : PlayerBaseState
    {
        public PlayerMoveState(CharacterObject character, PlayerController controller) : base(character, controller)
        {
        }
        public override void EnterState(PlayerStateManager player)
        {
            Move();
            player.SetCurrentStateEnum(PlayerStateEnum.Move);
            _character.GetCharacterAnimator().GetAnimator().SetInteger(StateEnum, (int)PlayerStateEnum.Move);
        }
        public override void UpdateState(PlayerStateManager player)
        {
        }
        
        public override void FixedUpdateState(PlayerStateManager player)
        {
            _controller.UpdateGravity();
            _controller.PrePhysicsUpdate();
            
            if (_controller.IsGrounded)
            {
                if (PlayerControls.Instance.jumpData.jumpPerformed)
                {
                    player.SwitchState(player.GetPlayerJumpState());
                    return;
                }
                if (PlayerControls.Instance.moveData.movePerformed == false)
                {
                    player.SwitchState(player.GetPlayerStoppingState());
                    return;
                }

                Move();
                return;
            }

            if (_controller.IsSteep)
            {
                player.SwitchState(player.GetPlayerSlidingState());
                return;
            }

            player.SwitchState(player.GetPlayerFallingState());

        }
        public override void LateUpdateState(PlayerStateManager player)
        {
            if(_controller.GetVelocity().x != 0f && _controller.GetVelocity().z != 0f)
                _character.RotateCharacter( _controller.GetVelocity(), _character.GetCharacterAttributes().GetCharacterMoveStats().GetTurnSpeed(),_controller.GetUpAxis());
        }

        public override void ExitState(PlayerStateManager player)
        {
            player.SetPreviousStateEnum(PlayerStateEnum.Move);
        }
        
        private void Move()
        {
            _controller.Move(_controller.GetVelocity(), _character.GetCharacterAttributes().GetCharacterMoveStats().GetAcceleration(), PlayerControls.Instance.moveData.moveValue, _character.GetCharacterAttributes().GetCharacterMoveStats().GetMoveSpeed());
        }
    }
}