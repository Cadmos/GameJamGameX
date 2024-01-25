using UnityEngine;

namespace FGJ24.Player
{
    public class PlayerMoveState : PlayerBaseState
    {
        private PlayerCharacter _character;
        private PlayerController _controller;

        
        private Vector3 _inputDirection;
        private float _movementSpeed;
        
        public PlayerMoveState(PlayerCharacter character, PlayerController controller)
        {
            _character = character;
            _controller = controller;
        }
        public override void EnterState(PlayerStateManager player)
        {
            
        }
        public override void UpdateState(PlayerStateManager player)
        {
            if (PlayerControls.Instance.moveData.MovePerformed == false)
                player.SwitchState(player.GetPlayerIdleState());

            _inputDirection = new Vector3(PlayerControls.Instance.moveData.MoveValue.x, 0, PlayerControls.Instance.moveData.MoveValue.y);
            _movementSpeed = _character.GetPlayerCharacterAttributes().GetPlayerMoveSpeed().GetMoveSpeed();

        }
        public override void FixedUpdateState(PlayerStateManager player)
        {
            _controller.Move(_inputDirection, _movementSpeed);
        }
        public override void LateUpdateState(PlayerStateManager player)
        {
            
        }
    }
}