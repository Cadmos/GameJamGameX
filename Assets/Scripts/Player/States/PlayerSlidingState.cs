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
            player.SetCurrentStateEnum(PlayerStateEnum.Sliding);
            _character.GetCharacterAnimator().GetAnimator().SetInteger(StateEnum, (int)PlayerStateEnum.Sliding);
        }

        public override void UpdateState(PlayerStateManager player)
        {
            if (_controller.GetIsGrounded() || _controller.IsSnapping)
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
                return;
            }

            if (!_controller.GetIsGrounded())
            {
                player.SwitchState(player.GetPlayerFallingState());
                return;
            }
                
        }

        public override void FixedUpdateState(PlayerStateManager player)
        {
            _controller.Sliding(_controller.GetVelocity(), _character.GetCharacterAttributes().GetCharacterMoveStats().GetAcceleration(), Vector2.zero, _character.GetCharacterAttributes().GetCharacterSlidingStats().GetSlidingSpeed());
            _controller.LimitVelocity(_character.GetCharacterAttributes().GetCharacterSlidingStats().GetSlidingSpeed());
        }

        public override void LateUpdateState(PlayerStateManager player)
        {
            if(_controller.GetVelocity().x != 0 || _controller.GetVelocity().z != 0)
                _character.ContactAlignedRotate(_controller.GetSteepNormal(), _controller.GetVelocity(), _character.GetCharacterAttributes().GetCharacterSlidingStats().GetSlidingTurnSpeed());
            
        }

        public override void ExitState(PlayerStateManager player)
        {
            player.SetPreviousStateEnum(PlayerStateEnum.Sliding);
        }
    }
}