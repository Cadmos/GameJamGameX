using UnityEngine;

namespace FGJ24.Player
{
    public class PlayerDashState : PlayerBaseState
    {
        public PlayerDashState(CharacterObject character, PlayerController controller) : base(character, controller)
        {
        }
        public override void EnterState(PlayerStateManager player)
        {
            player.SetCurrentStateEnum(PlayerStateEnum.Dash);
            _controller.SetIntentToDash(true);
            _character.GetCharacterAnimator().GetAnimator().SetInteger(StateEnum, (int)PlayerStateEnum.Dash);
            _character.GetCharacterAnimator().GetAnimator().SetFloat(DashAnimationSpeed, _character.GetCharacterAnimator().GetDashAnimation().length / _character.GetCharacterAttributes().GetCharacterDashStats().GetDashDuration());

        }
        public override void UpdateState(PlayerStateManager player)
        {
            if (_controller.GetIsGrounded())
            {
                if (_controller.GetIntentToDash())
                    return;

                if (_controller.GetDashStartTime() + _character.GetCharacterAttributes().GetCharacterDashStats().GetDashDuration() > Time.time)
                    return;
                
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
                player.SwitchState(player.GetPlayerSlidingState());
                return;
            }

            player.SwitchState(player.GetPlayerFallingState());
        }
        public override void FixedUpdateState(PlayerStateManager player)
        {
            if (_controller.GetIntentToDash())
            {
                _controller.Move( _controller.GetVelocity(), _character.GetCharacterAttributes().GetCharacterDashStats().GetInitialDashAcceleration(), _controller.GetDesiredVelocity(), _character.GetCharacterAttributes().GetCharacterDashStats().GetDashSpeed());
                _controller.LimitVelocity(_character.GetCharacterAttributes().GetCharacterDashStats().GetDashMaxSpeed());
                _controller.SetIntentToDash(false);
                _controller.SetNextDashTime(Time.time + _character.GetCharacterAttributes().GetCharacterDashStats().GetDashCooldownDuration() + _character.GetCharacterAttributes().GetCharacterDashStats().GetDashDuration());
                _controller.SetDashStartTime(Time.time);
                return;
            }
            _controller.Move(_controller.GetVelocity(), _character.GetCharacterAttributes().GetCharacterDashStats().GetDashAcceleration(), _controller.GetDesiredVelocity(), _character.GetCharacterAttributes().GetCharacterDashStats().GetDashSpeed());
            _controller.LimitVelocity(_character.GetCharacterAttributes().GetCharacterDashStats().GetDashMaxSpeed());
        }
        public override void LateUpdateState(PlayerStateManager player)
        {
            _character.RotateCharacter( _controller.GetVelocity(), _character.GetCharacterAttributes().GetCharacterDashStats().GetDashTurnSpeed(),_controller.GetUpAxis());
        }

        public override void ExitState(PlayerStateManager player)
        {
            player.SetPreviousStateEnum(PlayerStateEnum.Dash);
        }
    }
}