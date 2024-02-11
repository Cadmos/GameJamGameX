using System;
using UnityEngine;

namespace FGJ24.Player
{
    public class PlayerLandingState : PlayerBaseState
    {
        public PlayerLandingState(CharacterObject character, PlayerController controller) : base(character, controller)
        {
        }

        public override void EnterState(PlayerStateManager player)
        {
            player.SetCurrentStateEnum(PlayerStateEnum.Landing);
            _character.GetCharacterAnimator().GetAnimator().SetInteger(StateEnum, (int)PlayerStateEnum.Landing);
            _controller.SetLandingTime(Time.time);
        }

        public override void UpdateState(PlayerStateManager player)
        {
            if (_controller.GetIsGrounded())
            {
                if(_controller.HaveWeWon())
                    player.SwitchState(player.GetPlayerWinState());
            
                if(_controller.HaveWeLost())
                    player.SwitchState(player.GetPlayerDieState());
                
                if (PlayerControls.Instance.interactData.interactPerformed)
                    player.SwitchState(player.GetPlayerGatherState());

                if (PlayerControls.Instance.jumpData.jumpPerformed && _controller.GetNextJumpTime() <= Time.time)
                    player.SwitchState(player.GetPlayerJumpState());

                if (PlayerControls.Instance.dashData.dashPerformed && _controller.GetNextDashTime() <= Time.time)
                    player.SwitchState(player.GetPlayerDashState());
                
                if (PlayerControls.Instance.moveData.movePerformed)
                    player.SwitchState(player.GetPlayerMoveState());

                if (_controller.GetLandingTime() + _character.GetCharacterAttributes().GetCharacterLandingStats().GetLandingDuration() < Time.time)
                    return;
                
                player.SwitchState(player.GetPlayerIdleState()); 
                return;
            }

            if (_controller.GetIsSteep())
            {
                player.SwitchState(player.GetPlayerSlidingState());
                return;
            }
            player.SwitchState(player.GetPlayerFallingState());

        }

        public override void FixedUpdateState(PlayerStateManager player)
        {
            _controller.AdjustVelocity(_controller.GetVelocity(), _character.GetCharacterAttributes().GetCharacterLandingStats().GetLandingAcceleration(), _controller.GetDesiredVelocity(),false);
        }

        public override void LateUpdateState(PlayerStateManager player)
        {
        }

        public override void ExitState(PlayerStateManager player)
        {
            player.SetPreviousStateEnum(PlayerStateEnum.Landing);
        }
    }
}