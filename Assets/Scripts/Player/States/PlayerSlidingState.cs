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
            Sliding();
            player.SetCurrentStateEnum(PlayerStateEnum.Sliding);
            _character.GetCharacterAnimator().GetAnimator().SetInteger(StateEnum, (int)PlayerStateEnum.Sliding);
        }

        public override void UpdateState(PlayerStateManager player)
        {

        }

        public override void FixedUpdateState(PlayerStateManager player)
        {
            _controller.UpdateGravity();
            _controller.UpdatePhysicsState();
            
            if (_controller.GetIsGrounded())
            {
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
                Sliding();
                return;
            }
            
            player.SwitchState(player.GetPlayerFallingState());

        }

        public override void LateUpdateState(PlayerStateManager player)
        {
            /*
            if(_controller.GetVelocity().x != 0 || _controller.GetVelocity().z != 0)
                _character.ContactAlignedRotate(_controller.GetSteepNormal(), _controller.GetVelocity(), _character.GetCharacterAttributes().GetCharacterSlidingStats().GetSlidingTurnSpeed());
            */
        }

        public override void ExitState(PlayerStateManager player)
        {
            player.SetPreviousStateEnum(PlayerStateEnum.Sliding);
        }

        private void Sliding()
        {
            _controller.Sliding(_controller.GetVelocity(), _character.GetCharacterAttributes().GetCharacterMoveStats().GetAcceleration(), Vector2.zero, _character.GetCharacterAttributes().GetCharacterSlidingStats().GetSlidingSpeed());
            //_controller.LimitVelocity(_character.GetCharacterAttributes().GetCharacterSlidingStats().GetSlidingSpeed());
        }
    }
}