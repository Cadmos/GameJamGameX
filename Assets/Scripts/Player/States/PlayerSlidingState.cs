using System;
using UnityEngine;

namespace FGJ24.Player
{
    public class PlayerSlidingState : PlayerBaseState
    {
        public PlayerSlidingState(CharacterObject character, PlayerController controller) : base(character, controller)
        {
        }

        public override void EnterState(PlayerStateManager player)
        {
            _character.GetCharacterAnimator().GetAnimator().SetInteger(StateEnum, (int)PlayerStateEnum.Sliding);
            //_controller.SetMaxSnapSpeed(_character.GetCharacterAttributes().GetCharacterSlidingStats().GetSlidingSpeed()+8);
        }

        public override void UpdateState(PlayerStateManager player)
        {
            
            if (_controller.GetIsGrounded())
            {
                if (_controller.HaveWeWon())
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
                   

                if (PlayerControls.Instance.moveData.movePerformed)
                {
                     player.SwitchState(player.GetPlayerMoveState());
                    return;
                }

                player.SwitchState(player.GetPlayerStoppingState());
                return;
            }
            
            if (_controller.GetIsSteep())
            {
                _controller.SetDesiredVelocity(_controller.GetSteepNormal()*_character.GetCharacterAttributes().GetCharacterSlidingStats().GetSlidingSpeed());
                return;
            }


            
            player.SwitchState(player.GetPlayerFallingState());
        }

        public override void FixedUpdateState(PlayerStateManager player)
        {
            _controller.AdjustVelocity(_controller.GetVelocity(), _character.GetCharacterAttributes().GetCharacterSlidingStats().GetSlidingAcceleration(), _controller.GetDesiredVelocity());
            _controller.LimitVelocity(_character.GetCharacterAttributes().GetCharacterSlidingStats().GetSlidingSpeed());
        }

        public override void LateUpdateState(PlayerStateManager player)
        {
            _character.RotateCharacter( _controller.GetVelocity(), _character.GetCharacterAttributes().GetCharacterSlidingStats().GetSlidingTurnSpeed());
        }

        public override void ExitState(PlayerStateManager player)
        {
            player.SetPreviousStateEnum(PlayerStateEnum.Sliding);
        }
    }
}