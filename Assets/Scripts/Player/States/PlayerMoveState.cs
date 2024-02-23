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
            player.SetCurrentStateEnum(PlayerStateEnum.Move);
            _character.GetCharacterAnimator().GetAnimator().SetInteger(StateEnum, (int)PlayerStateEnum.Move);
        }
        public override void UpdateState(PlayerStateManager player)
        {
            if(_controller.IsClimbing)
            {
                player.SwitchState(player.GetPlayerClimbState());
                return;
            }
            
            if (_controller.WasGroundedLastFrame || _controller.GetIsGrounded() || _controller.IsSnapping)
            {
                if (_controller.HaveWeWon())
                {
                    player.SwitchState(player.GetPlayerWinState());
                    return;
                }
                if (_controller.HaveWeLost())
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
                return;
            }

            if (_controller.GetIsSteep())
            {
                player.SwitchState(player.GetPlayerSlidingState());
                return;
            }

            if (!_controller.GetIsGrounded() && !_controller.WasGroundedLastFrame && !_controller.IsSnapping)
            {
                player.SwitchState(player.GetPlayerFallingState());
            }
                
        }
        public override void FixedUpdateState(PlayerStateManager player)
        {
            
            _controller.Move(_controller.GetVelocity(), _character.GetCharacterAttributes().GetCharacterMoveStats().GetAcceleration(), PlayerControls.Instance.moveData.moveValue, _character.GetCharacterAttributes().GetCharacterMoveStats().GetMoveSpeed());
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
    }
}