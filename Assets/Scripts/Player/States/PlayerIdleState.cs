using UnityEngine;

namespace FGJ24.Player
{
    public class PlayerIdleState : PlayerBaseState
    {
        public PlayerIdleState(CharacterObject character, PlayerController controller) : base(character, controller)
        {
        }

        public override void EnterState(PlayerStateManager player)
        {
            player.SetCurrentStateEnum(PlayerStateEnum.Idle);
            _character.GetCharacterAnimator().GetAnimator().SetInteger(StateEnum, (int)PlayerStateEnum.Idle);
        }

        public override void UpdateState(PlayerStateManager player)
        {
            if (_controller.WasGroundedLastFrame || _controller.GetIsGrounded())
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
                
                _controller.SetDesiredVelocity(Vector3.zero);
                
                return;
            }

            if (_controller.GetIsSteep())
            {
                player.SwitchState(player.GetPlayerSlidingState());
                return;
            }

            if (!_controller.GetIsGrounded())
            {
                player.SwitchState(player.GetPlayerFallingState());
                return;
            }
            
            Debug.LogWarning("for some reason we are not grounded, but we are in idle state. This should not happen.");
            
        }

        public override void FixedUpdateState(PlayerStateManager player)
        {
            _controller.Move(_controller.GetVelocity(), _character.GetCharacterAttributes().GetCharacterIdleStats().GetIdleAcceleration(), _controller.GetDesiredVelocity());
        }

        public override void LateUpdateState(PlayerStateManager player)
        {
            if(_controller.GetVelocity().x != 0f && _controller.GetVelocity().z != 0f)
                _character.RotateCharacter( _controller.GetVelocity(), _character.GetCharacterAttributes().GetCharacterIdleStats().GetIdleTurnSpeed());
        }

        public override void ExitState(PlayerStateManager player)
        {
            player.SetPreviousStateEnum(PlayerStateEnum.Idle);
        }
    }
}