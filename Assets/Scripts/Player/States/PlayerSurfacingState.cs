using UnityEngine;

namespace FGJ24.Player
{
    public class PlayerSurfacingState : PlayerBaseState
    {
        public PlayerSurfacingState(CharacterObject character, PlayerController controller) : base(character, controller)
        {
        }

        public override void EnterState(PlayerStateManager player)
        {
            player.SetCurrentStateEnum(PlayerStateEnum.Swim);
            _character.GetCharacterAnimator().GetAnimator().SetInteger(StateEnum, (int)PlayerStateEnum.Swim);
        }

        public override void UpdateState(PlayerStateManager player)
        {
            if(_controller.InWater)
            {
                if(_controller.Swimming)
                {
                    return;
                }
            }
            
            if(_controller.IsClimbing)
            {
                player.SwitchState(player.GetPlayerClimbState());
                return;
            }
            
            if (_controller.IsGrounded)
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

            if (_controller.IsSteep)
            {
                player.SwitchState(player.GetPlayerSlidingState());
                return;
            }

            if (!_controller.InWater)
            {
                player.SwitchState(player.GetPlayerFallingState());
            }
                
        }

        public override void FixedUpdateState(PlayerStateManager player)
        {
            _controller.Swim(_controller.GetVelocity(), _character.GetCharacterAttributes().GetCharacterSwimStats().GetAcceleration(), PlayerControls.Instance.moveData.moveValue, _character.GetCharacterAttributes().GetCharacterSwimStats().GetSwimSpeed());
        }

        public override void LateUpdateState(PlayerStateManager player)
        {
            
        }

        public override void ExitState(PlayerStateManager player)
        {
            player.SetPreviousStateEnum(PlayerStateEnum.Swim);
        }
    }
}