using System;
using UnityEngine;

namespace FGJ24.Player
{
    public class PlayerStoppingState : PlayerBaseState
    {
        public PlayerStoppingState(CharacterObject character, PlayerController controller) : base(character, controller)
        {
        }

        public override void EnterState(PlayerStateManager player)
        {
            _character.GetCharacterAnimator().GetAnimator().SetInteger(StateEnum, (int)PlayerStateEnum.Stopping);
        }

        public override void UpdateState(PlayerStateManager player)
        {
            _controller.UpdateDesiredVelocity(_character.GetCharacterAttributes().GetCharacterStoppingStats().GetStoppingSpeed());

            if (_controller.GetIsGrounded())
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

                if (PlayerControls.Instance.jumpData.jumpPerformed && _controller.GetNextJumpTime() <= Time.time)
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

                if (_controller.GetVelocity().magnitude < 0.1f)
                {
                    player.SwitchState(player.GetPlayerIdleState());
                    return;
                }


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
            _controller.AdjustVelocity(_controller.GetVelocity(), _character.GetCharacterAttributes().GetCharacterStoppingStats().GetStoppingAcceleration(), Vector3.zero);
        }

        public override void LateUpdateState(PlayerStateManager player)
        {
            _character.RotateCharacter(_controller.GetVelocity(), _character.GetCharacterAttributes().GetCharacterStoppingStats().GetStoppingTurnSpeed());
        }

        public override void ExitState(PlayerStateManager player)
        {
            player.SetPreviousStateEnum(PlayerStateEnum.Stopping);
        }
    }
}