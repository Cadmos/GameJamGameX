using UnityEngine;

namespace FGJ24.Player
{
    public class PlayerIdleState : PlayerBaseState
    {
        private PlayerCharacter _character;
        private PlayerController _controller;
        
        public PlayerIdleState(PlayerCharacter character, PlayerController controller)
        {
            _character = character;
            _controller = controller;
        }

        public override void EnterState(PlayerStateManager player)
        {
            
        }
        public override void UpdateState(PlayerStateManager player)
        {
            Debug.Log("PlayerIdleState.UpdateState");
            if (PlayerControls.Instance.moveData.MovePerformed)
            {
                player.SwitchState(player.GetPlayerMoveState());
            }
        }
        public override void FixedUpdateState(PlayerStateManager player)
        {
            
        }
        public override void LateUpdateState(PlayerStateManager player)
        {
            
        }
    }
}